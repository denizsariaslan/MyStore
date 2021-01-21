using MyStore.Core.Contracts;
using MyStore.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MyStore.WebUI.Controllers
{
    // [Authorize(Roles = "Admin")] //Authorization for Admin
    public class OrderManagerController : Controller
    {
        IOrderService orderService; //Expecting

        //Constructor
        public OrderManagerController(IOrderService OrderService)
        {
            this.orderService = OrderService;
        }

        // GET: OrderManager
        public ActionResult Index() //Returning list of orders
        {
            List<Order> orders = orderService.GetOrderList();

            return View(orders);
        }

        [HttpPost]
        public ActionResult Search(string searchtext)
        {
            List<Order> orders = orderService.GetOrderList().Where(p => p.FirstName.Contains(searchtext) || p.LastName.Contains(searchtext)).ToList();
            if (orders == null)
            {
                return HttpNotFound();
            }
            else
            {
                return View("Index", orders);
            }
        }

        public ActionResult UpdateOrder(string Id) //getting an individual order
        {
            //in the order status I don't want people to or I don't want our managers to manual inputs order status
            //That's the only change I need to make in the controller
            ViewBag.StatusList = new List<string>() {
                "Order Created",
                "Payment Processed",
                "Order Shipped",
                "Order Complete"
            };
            Order order = orderService.GetOrder(Id);
            return View(order);
        }

        [HttpPost] //it can say the difference between the one is returning the page and the one that's being updated
        public ActionResult UpdateOrder(Order updatedOrder, string Id) //Receive updated order
        {
            Order order = orderService.GetOrder(Id);

            // I just want to update order status that's the only item that I will change
            //Doing it this way I just have a bit more control to make sure that a user can't accidentally update
            //something there shouldn't be updating.

            order.OrderStatus = updatedOrder.OrderStatus; 
            orderService.UpdateOrder(order);

            return RedirectToAction("Index"); //returning list of orders
        }
    }
}