using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyStore.Core.ViewModels
{
   public class CartItemViewModel
    {
        public string Id { get; set; }
        [DisplayName("Quantity")]
        public int Quanity { get; set; }
        [DisplayName("Product Name")]
        public string ProductName { get; set; }
        public decimal Price { get; set; }
        public string Image { get; set; }
    }
}
