using MyStore.Core.Contracts;
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
        //local instance of ICartService
        ICartService cartService;
        //constructor allow to inject in the cart service
        public CartController(ICartService CartService)
        {
            this.cartService = CartService;
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

            return RedirectToAction("Index");
        }

        //Remove into Cart
        public ActionResult RemoveFromCart(string Id)
        {
            //different from addtocart is Id is CartItem Id
            cartService.RemoveFromCart(this.HttpContext, Id);

            return RedirectToAction("Index");
        }

        //Cart Summary
        // Cart summarry is going to be partial view

        public PartialViewResult CartSummary()
        {
            var cartSummary = cartService.GetCartSummary(this.HttpContext);

            return PartialView(cartSummary);
        }
    }

}