using MyStore.Core.Contracts;
using MyStore.Core.Models;
using MyStore.Core.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyStore.Services
{
    public class OrderService : IOrderService
    {
        IRepository<Order> orderContext;

        //Constructor
        public OrderService(IRepository<Order> OrderContext)
        {
            this.orderContext = OrderContext;
        }

        public void CreateOrder(Order baseOrder, List<CartItemViewModel> cartItems)
        {
            //iterate through our cartItems. For each cart item I am going to added to the underlying baseOrder.
           foreach (var item in cartItems)
            {
                baseOrder.OrderItems.Add(new OrderItem()
                {
                    ProductId = item.Id,
                    Image = item.Image,
                    Price = item.Price,
                    ProductName = item.ProductName,
                    Quanity = item.Quanity
                });
            }

            orderContext.Insert(baseOrder); //Insert baseOrder
            orderContext.Commit(); //Save the changes
        }

        // Added this new method according to order management process
        public List<Order> GetOrderList() //this will return our list of orders.
        {
            return orderContext.Collection().ToList();
        }
        // Added this new method according to order management process
        public Order GetOrder(string Id) //this will return an individual order.
        {
            return orderContext.Find(Id);
        }
        // Added this new method according to order management process
        public void UpdateOrder(Order updatedOrder) // this will update our order. // I want to update our Order will be the order status.
        {
            orderContext.Update(updatedOrder);
            orderContext.Commit();
        }
    }
}
