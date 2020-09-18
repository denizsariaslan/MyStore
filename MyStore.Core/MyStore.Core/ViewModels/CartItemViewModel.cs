using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyStore.Core.ViewModels
{
   public class CartItemViewModel
    {
        public string Id { get; set; }
        public int Quanity { get; set; }
        public string ProductName { get; set; }
        public decimal Price { get; set; }
        public string Image { get; set; }
    }
}
