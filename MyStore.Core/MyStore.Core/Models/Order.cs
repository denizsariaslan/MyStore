using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyStore.Core.Models
{
    public class Order: BaseEntity
    {
        //Constructor
        public Order()
        {
            // I want to make sure the OrderItems list is initialised. Otherwise when I try to add products to
            //the list it would throw an error.
            this.OrderItems = new List<OrderItem>();
        }

        //I am going to keep a copy of the customer details. Again I could link directly to the customer themselves.
        //However I find it often handy to link to copy the details just because sometimes a user might want
        //to date their personal details within their actual records or for example they might want to have a
        //separate billing address in shipping addresses.
        //So by storing some of these details separately within the order it just gives us a little bit more flexibility.

        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; } 
        public string Street { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Country { get; set; }
        public string ZipCode { get; set; }
        public string OrderStatus { get; set; } //Adding an order status here. This is so that I manage out the order itself so for example when I go from raising in order to processing payment to shipping the payment and so on I can keep track of where I am at.
        public virtual ICollection<OrderItem> OrderItems { get; set; } //Creating a list of items this fall tell entity framework to link the two together so that when I load an order I will automatically get the items.



    }
}
