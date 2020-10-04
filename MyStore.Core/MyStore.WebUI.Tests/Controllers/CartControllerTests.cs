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
            IRepository<Order> orders = new MockContext<Order>();// creating MockContext for Order

            var httpContext = new MockHttpContext();


            ICartService cartService = new CartService(products, carts);
            IOrderService orderService = new OrderService(orders); // Creating order service
            var controller = new CartController(cartService, orderService);//Creating instance of controller //orderService added for test
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
            IRepository<Order> orders = new MockContext<Order>();// creating MockContext for Order

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
            IOrderService orderService = new OrderService(orders); // Creating order service

            var controller = new CartController(cartService, orderService); //Creating controller //orderService added for test
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
        [TestMethod]
        public void CanCheckOutAndCreateOrder()
        {
            ////Setup

            IRepository<Product> products = new MockContext<Product>(); //Mock product repository created

            //Adding some basic information in repository
            //Added Id and Price because that's the only thing that will affect any of calculations
            products.Insert(new Product() { Id = "1", Price = 10.00m });
            products.Insert(new Product() { Id = "2", Price = 8.00m });
            products.Insert(new Product() { Id = "3", Price = 13.00m });


            IRepository<Cart> carts = new MockContext<Cart>();//Creating Mock Cart repository
            Cart cart = new Cart(); //New underlying Cart
            //Adding some basic information
            cart.CartItems.Add(new CartItem() { ProductId = "1", Quanity = 2, CartId = cart.Id });
            cart.CartItems.Add(new CartItem() { ProductId = "1", Quanity = 1, CartId = cart.Id });
            cart.CartItems.Add(new CartItem() { ProductId = "1", Quanity = 3, CartId = cart.Id });

            //I need to add that cart to the cart repository
            carts.Insert(cart);

            //Creating CartService
            ICartService cartService = new CartService(products, carts);

            //Creating OrderService first I need again repository of order
            IRepository<Order> orders = new MockContext<Order>();
            IOrderService orderService = new OrderService(orders);

            //if I wanted to test the order service directly. I could simply act on the order service itself.
            //but I am going to test controller again

            //Adding additional information into create a CartController instance

            var controller = new CartController(cartService, orderService); //controller expect cartService and OrderService
            var httpContext = new MockHttpContext();//Injecting fake context so that it can read an write cookies
            //Creating cookie itself manually because i am creating cart manually
            httpContext.Request.Cookies.Add(new System.Web.HttpCookie("eCommerceCart")
            {
                Value = cart.Id
            });

            //As last one I need to add httpContext to the underlying ControllerContext
            controller.ControllerContext = new ControllerContext(httpContext, new System.Web.Routing.RouteData(),controller);


            ////Act

            Order order = new Order();//creating order
            controller.CheckOut(order);

            ////Assert
            Assert.AreEqual(3, order.OrderItems.Count); //testing order items
            Assert.AreEqual(0, cart.CartItems.Count); //Make sure I have created an order and I want to clear the cart down

            Order orderInRep = orders.Find(order.Id); //try and retrieve the order from the repository.
            Assert.AreEqual(3, orderInRep.OrderItems.Count);//check 3 order items in the repository itself


        }
    }
}
