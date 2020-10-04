using MyStore.Core.Contracts;
using MyStore.Core.Models;
using MyStore.Core.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace MyStore.Services
{
    public class CartService : ICartService
    {
        //Need these repository; access to some underlying data and load actual cart and cart items
        IRepository<Product> productContext; 
        IRepository<Cart> cartContext;

        //when we're written in writing cookies we use a string to identify a particular cookie we want
        // and just so that we can enforce consistency. Whether we're reading in that cookie we will do store it in a string
        //and we can use that string to reference it each time instead we set it as a public const which basically means this string to not be updated elsewhere within our code.

        public const string CartSessionName = "eCommerceCart";

        //create constructure
        public CartService(IRepository<Product> ProductContext, 
        IRepository<Cart> CartContext)
        {
            this.productContext = ProductContext;
            this.cartContext = CartContext;
        }

        //The first method is loading the Cart.
        //I want to read uses cookies from the HTTP context looking to the CartId and then attempt to read that CartId in the database.
        //I used private method because this will be an internal method because I want to use this method from various places within our service.
        //I want to have access to the uses Http context.I will force the user or rather I will force the consuming service to send this http context in this part of the Column.
        //Also going to have a bool called createIfNull that this is because sometimes we will want to Cart be created if none exists and other towns we want.
        private Cart GetCart(HttpContextBase httpContext, bool createIfNull)
        {
            //the first step is we need to try to read the cookie
            HttpCookie cookie = httpContext.Request.Cookies.Get(CartSessionName);

            //I would create a new cart.Then checking to see if the cookie actually exists.

            Cart cart = new Cart();

            if(cookie != null)
            {
                string cartId = cookie.Value;
                if (!string.IsNullOrEmpty(cartId))
                {
                    cart = cartContext.Find(cartId);
                }
                else
                {
                    if (createIfNull)
                    {
                        cart = CreateNewCart(httpContext);
                    }
                }
            }
            else {
                if (createIfNull)
                {
                    cart = CreateNewCart(httpContext);
                }
            }
            return cart;

        }

        //CreateNewCart method and this will be private or internal mechanizm

        private Cart CreateNewCart(HttpContextBase httpContext)
        {
            Cart cart = new Cart();
            cartContext.Insert(cart); //Insert into database
            cartContext.Commit();

            //Writing cookie to use machine

            HttpCookie cookie = new HttpCookie(CartSessionName); //First create a cookie
            cookie.Value = cart.Id; // adding value
            cookie.Expires = DateTime.Now.AddDays(1); // setting expiration on that cookie
            httpContext.Response.Cookies.Add(cookie);// adding cookie to the http contex responses because the responses who I am sending back to the user

            return cart;
        }
        // I setted above both these methods are private because I will only use them internally with in CartService class

        //Add Cart item method

        public void AddToCart(HttpContextBase httpContext, string productId)
        {
            Cart cart = GetCart(httpContext, true);//I wanna know what the CartId is and inserting an item always we need to make sure Cart is created and create is no need to be true.

            //if there's already a cart item in the uses cart with productId
            //Because we've got the basket from the database and entity framework using because 
            //we use the virtual keyboard on the list of Cart items will automatically load all those Cart items for as when ever I try to load cart items from database

            CartItem item = cart.CartItems.FirstOrDefault(i => i.ProductId == productId);
            //if that item exist in the cart

            if (item == null)
            {
                //If it doesn't then we want to create a new item.
                item = new CartItem()
                {
                    CartId = cart.Id,
                    ProductId = productId,
                    Quanity = 1
                };

                // added underlying cart items
                cart.CartItems.Add(item);
            }
            else
            {
                //increment our property
                item.Quanity = item.Quanity + 1;
            }

            //Commit any changes I have made.
            cartContext.Commit();
        }

        //Remove Cart item method
        public void RemoveFromCart(HttpContextBase httpContext, string itemId)
        {
            Cart cart = GetCart(httpContext, true);
            CartItem item = cart.CartItems.FirstOrDefault(i => i.Id == itemId);

            if (item !=null)
            {
                cart.CartItems.Remove(item);
                cartContext.Commit();
            }
            
        }
        // new method GetCartItems for CartItemViewModel
        public List<CartItemViewModel> GetCartItems(HttpContextBase httpContext)
        {
            //Get our Cart from database
            //I am just retrieving items if the Cart does not touch ... exist do not want it go ahead and create new one.
            //if there is no items in the Cart to the Moment will simply return an empty in memory Cart.

            Cart cart = GetCart(httpContext, false);

            //if we have retrieve the cart then I need to query the product table and the cart items to get the information that I need.
            //I used linq
            //This method will return list of all items in the cart which is great for when a user clicks to look at the cart page.

            if (cart != null)
            {
                var results = (from b in cart.CartItems
                               join p in productContext.Collection() on b.ProductId equals p.Id
                               select new CartItemViewModel()
                               {
                                   Id = b.Id,
                                   Quanity = b.Quanity,
                                   ProductName = p.Name,
                                   Image = p.Image,
                                   Price = p.Price
                               }
                              ).ToList();

                return results;
            }
            else
            {
                return new List<CartItemViewModel>();
            }
        }

        ////Provide a Cart summary which will simply be a total list of all the items in a Cart and a total quantity in the Cart. for this I need a another view model

        public CartSummaryViewModel GetCartSummary(HttpContextBase httpContext)
        {
            //The first thing I need to do is get my cart and again as before I want to not create the cart if its currently empty.
            Cart cart = GetCart(httpContext, false);
            //create our view model and default it (0,0). this is why I created construct solved by default zero item zero cart total
            CartSummaryViewModel model = new CartSummaryViewModel(0, 0);
            if (cart != null)
            {
                //The first calculate how many items are in the cart. I used linq query
                //select just the quantities from each item our cart and then sum them up.
                int? cartCount = (from item in cart.CartItems
                                    select item.Quanity).Sum();

                decimal? cartTotal = (from item in cart.CartItems
                                        join p in productContext.Collection() on item.ProductId equals p.Id
                                        select item.Quanity * p.Price).Sum();

                //the final step is to assign these values to our model.
                model.CartCount = cartCount ?? 0;
                model.CartTotal = cartTotal ?? decimal.Zero;

                return model;
            }
            else
            {
                return model;
            }



        }

        //Adding ClearCart method
        public void ClearCart(HttpContextBase httpContext) 
        {
            Cart cart = GetCart(httpContext, false); //sending httpContext to get the cart
            cart.CartItems.Clear(); //Clear method which is the part of the linq method
            cartContext.Commit();
        }


    }
}
