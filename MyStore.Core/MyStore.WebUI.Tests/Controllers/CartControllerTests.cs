using System;
using System.Linq;
using System.Web.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MyStore.Core.Contracts;
using MyStore.Core.Models;
using MyStore.Core.ViewModels;
using MyStore.Services;
using MyStore.WebUI.Controllers;
using MyStore.WebUI.Tests.Mocks;

namespace MyStore.WebUI.Tests.Controllers
{
    [TestClass]
    public class CartControllerTests
    {
        [TestMethod]
        public void CanAddCartItem()
        {
            // Testing it via controller that way I get a double test
            //I get to test the service itself and we get to test the control itself.
            //Setup
            //Creating repositories
            IRepository<Cart> carts = new MockContext<Cart>();
            IRepository<Product> products = new MockContext<Product>();

            var httpContext = new MockHttpContext();


            ICartService cartService = new CartService(products, carts);
            var controller = new CartController(cartService);//Creating instance of controller
            controller.ControllerContext = new System.Web.Mvc.ControllerContext(httpContext, new System.Web.Routing.RouteData(), controller); //Inject httpContext. (I need to specify ControllerContext is a new ControllerContext and send in our Mock context 

            //Act
            //basketService.AddToBasket(httpContext, "1");
            controller.AddToCart("1"); //This method creating cookies and carts itself
            
            Cart cart = carts.Collection().FirstOrDefault();//Chechking any Carts in our Cart collection


            //Assert
            //Test and check if everything works
            Assert.IsNotNull(cart); //Checking Cart itself is not null
            Assert.AreEqual(1, cart.CartItems.Count);//Chechking if I have one item in Cart
            Assert.AreEqual("1", cart.CartItems.ToList().FirstOrDefault().ProductId); //Chechking productID in the cart is the one I have just injected 

            ////Testing actual CartService directly
            ////setup
            //IRepository<Cart> carts = new MockContext<Cart>();
            //IRepository<Product> products = new MockContext<Product>();

            //var httpContext = new MockHttpContext();

            //ICartService cartService = new CartService(products, carts);
            ////Act
            //basketService.AddToBasket(httpContext, "1");
            //Cart cart = carts.Collection().FirstOrDefault();
            ////Assert
            //Assert.IsNotNull(cart);
            //Assert.AreEqual(1, cart.CartItems.Count);
            //Assert.AreEqual("1", cart.CartItems.ToList().FirstOrDefault().ProductId);
        }

        [TestMethod]
        public void CanGetSummaryViewModel()
        {
            
            ////Setup
            IRepository<Cart> carts = new MockContext<Cart>();
            IRepository<Product> products = new MockContext<Product>();

            //Consantiration on calculations 
            //Manually adding some items into products database 
            //Because what I am testing is not how to add up products just what gets returned when we call the Get summary method.
            products.Insert(new Product() { Id = "1", Price = 10.00m });
            products.Insert(new Product() { Id = "2", Price = 5.00m });

            //Creating cart and add items to it.
            //Because I need to make sure that the item to it running to the cart have a corresponding Id and price 
            //so that our underlying GetSummeryViewModel method has all the information it needs.
            Cart cart = new Cart();
            cart.CartItems.Add(new CartItem() { ProductId = "1", Quanity = 2 });
            cart.CartItems.Add(new CartItem() { ProductId = "2", Quanity = 1 });
            carts.Insert(cart);

            ICartService cartService = new CartService(products, carts);

            var controller = new CartController(cartService); //Creating controller
            var httpContext = new MockHttpContext();
            httpContext.Request.Cookies.Add(new System.Web.HttpCookie("eCommerceCart") { Value = cart.Id }); //Manually add cookies 
            controller.ControllerContext = new System.Web.Mvc.ControllerContext(httpContext, new System.Web.Routing.RouteData(), controller); //Inject httpContext(cookies).


            var result = controller.CartSummary() as PartialViewResult; //Because I exposed the controller as a partial view.
            var cartSummary = (CartSummaryViewModel)result.ViewData.Model;

            //Assert
            //Testing
            Assert.AreEqual(3, cartSummary.CartCount);
            Assert.AreEqual(25.00m, cartSummary.CartTotal);


        }
    }
}
