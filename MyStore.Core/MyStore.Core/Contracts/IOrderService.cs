using MyStore.Core.Models;
using MyStore.Core.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyStore.Core.Contracts
{
    public interface IOrderService
    {
        //It's going to want to take in an order object which would get created externally from the create orderservice
        // because it's simply a model and then I am going to bring in a listed CartItemViewModel.

        void CreateOrder(Order baseOrder, List<CartItemViewModel> cartItems);
    }
}
