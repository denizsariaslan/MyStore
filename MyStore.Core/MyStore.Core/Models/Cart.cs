using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyStore.Core.Models
{
   public class Cart : BaseEntity
    {
        //I set this is a virtual ICollection. This is important for the Entity framework.
        //But setting is a virtual ICollection entity framework that will know that whenever we try to load the Cart from the database,
        //it will automatically load all the Cart items as well. This is what's known as lazy loading.
        public virtual ICollection<CartItem> CartItems { get; set; }

        //Constructor that will create an empty list of basket items of creation.
        public Cart()
        {
            this.CartItems = new List<CartItem>();
        }

    }
}
