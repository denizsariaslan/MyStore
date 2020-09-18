using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyStore.Core.ViewModels
{
    public class CartSummaryViewModel
    {
        public int CartCount { get; set; }
        public decimal CartTotal { get; set; }

        //Creating couple of constructor
        
        public CartSummaryViewModel()
        {

        }
       
        public CartSummaryViewModel(int cartCount, decimal cartTotal)
        {
            //setting default values
            this.CartCount = cartCount;
            this.CartTotal = cartTotal;
        }


    }

   

}
