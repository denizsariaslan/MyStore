using MyStore.Core.Contracts;
using MyStore.Core.Models;
using MyStore.Core.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MyStore.WebUI.Controllers
{
    public class CartController : Controller
    {
        //local instance of ICartService
        ICartService cartService;
        //Adding IOrderService for OrderModels process
        IOrderService orderService;

        //constructor allow to inject in the cart service
        public CartController(ICartService CartService, IOrderService OrderService) //Adding IOrderService for OrderModels process
        {
            this.cartService = CartService;
            this.orderService = OrderService; //Adding IOrderService for OrderModels process
        }
        // GET: Cart
        public ActionResult Index()
        {
            //I used the main index page for returning Cart view which will a list of all cart items

            var model = cartService.GetCartItems(this.HttpContext);
            return View(model);
        }

        //POST
        // Adding into Cart
        public ActionResult AddToCart(string Id)
        {
            //Take in a product Id and pass that through to the cart service.
            cartService.AddToCart(this.HttpContext, Id);

            return RedirectToAction("Index");
        }

        //Remove into Cart
        public ActionResult RemoveFromCart(string Id)
        {
            //different from addtocart is Id is CartItem Id
            cartService.RemoveFromCart(this.HttpContext, Id);

            return RedirectToAction("Index");
        }

        //Cart Summary
        // Cart summarry is going to be partial view

        public PartialViewResult CartSummary()
        {
            var cartSummary = cartService.GetCartSummary(this.HttpContext);

            return PartialView(cartSummary);
        }

        //Adding Checkout page for OrderModels process
        public ActionResult CheckOut()
        {
            return View();
        }

        //posting actual ChechkOut page itself for OrderModels process
        [HttpPost]
        public ActionResult CheckOut(Order order)
        {
            var cartItems = cartService.GetCartItems(this.HttpContext);
            order.OrderStatus = "Order Created"; //Update the status

            //Payment process

            order.OrderStatus = "Payment Processed";
            orderService.CreateOrder(order, cartItems); //sending our base order and cartItems
            cartService.ClearCart(this.HttpContext); //clear our cart down

            //Redirect them to a thank you page
            return RedirectToAction("Thankyou", new { OrderId = order.Id }); //thankyou page also send in orderId

        }
        //Creating Thank you page for OrderModels process
        public ActionResult ThankYou(string OrderId) //return to the user what that orderId is
        {
            
            ViewBag.OrderId = OrderId; //store that orderId in the Viewbag
            return View(); 
        }
    }

}