using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyStore.Core.Models
{
    public class CartItem : BaseEntity
    {
        // CartId will be the link back to the Cart that contains the Cart items
        public string CartId { get; set; }
        //a link to a ProductId
        public string ProductId { get; set; }
        [DisplayName("Quantity")]
        public int Quanity { get; set; }
    }

}
