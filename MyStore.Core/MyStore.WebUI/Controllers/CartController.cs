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
        ////Providing customer repository (linking customers to orders process)
        IRepository<Customer> customers;
        //local instance of ICartService
        ICartService cartService;
        //Adding IOrderService for OrderModels process
        IOrderService orderService;

        //constructor allow to inject in the cart service
        public CartController(ICartService CartService, IOrderService OrderService, IRepository<Customer> Customers) //Adding IOrderService for OrderModels process ////Providing customer repository (linking customers to orders process)
        {
            this.cartService = CartService;
            this.orderService = OrderService; //Adding IOrderService for OrderModels process
            this.customers = Customers; //Providing customer repository (linking customers to orders process)
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

            return RedirectToAction("Index","Home");
        }

        public ActionResult IncrementCartItem(string Id)
        {
            //Take in a product Id and pass that through to the cart service.
            cartService.IncrementCartItem(this.HttpContext, Id);

            return RedirectToAction("Index","Cart");
        }
        //Remove into Cart
        public ActionResult RemoveFromCart(string Id)
        {
            //different from addtocart is Id is CartItem Id
            cartService.RemoveFromCart(this.HttpContext, Id);

            return RedirectToAction("Index");
        }

        public ActionResult DecrementCartItem(string Id)
        {
            //different from addtocart is Id is CartItem Id
            cartService.DecrementCartItem(this.HttpContext, Id);

            return RedirectToAction("Index","Cart");
        }

        //Cart Summary
        // Cart summarry is going to be partial view

        public PartialViewResult CartSummary()
        {
            var cartSummary = cartService.GetCartSummary(this.HttpContext);

            return PartialView(cartSummary);
        }

        //Adding Checkout page for OrderModels process
        ////Updatig the first checkout actionresult and get the customers from the database(linking customers to orders process)
        [Authorize]////Make sure the user us logged in (linking customers to orders process)
        public ActionResult CheckOut()
        {
            Customer customer = customers.Collection().FirstOrDefault(c => c.Email == User.Identity.Name); //Using lambda expression to search the customer on their e-mail
            //make sure the customer is not null
            if (customer != null)
            {
                //Creating new order
                Order order = new Order()
                {
                    FirstName = customer.FirstName,
                    LastName = customer.LastName,
                    Street = customer.Street,
                    Email = customer.Email,
                    City = customer.City,
                    State = customer.State,
                    Country = customer.Country,
                    ZipCode = customer.ZipCode
                };
                return View(order);//ASP.Net page itself wiil taken object and apply it to the fields
            }
            //the customer is null it need to return error
            else
            {
                return RedirectToAction("Error");
            }

            

        }

        //posting actual ChechkOut page itself for OrderModels process
        [HttpPost]
        [Authorize]//Users have to be logged in (linking customers to orders process)
        public ActionResult CheckOut(Order order)
        {
            var cartItems = cartService.GetCartItems(this.HttpContext);
            order.OrderStatus = "Order Created"; //Update the status
            order.Email = User.Identity.Name;//adding this here to make sure that the user is logged in (linking customers to orders process)

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