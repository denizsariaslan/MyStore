using MyStore.Core.Contracts;
using MyStore.Core.Models;
using MyStore.Core.ViewModels;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.Mvc;

namespace MyStore.WebUI.Controllers
{
    public class HomeController : Controller
    {
        //    ProductRepository context;
        //    ProductCategoryRepository productCategories;

        //these two lines updated when generics implemented
        ////InMemoryRepository<Product> context;
        ////InMemoryRepository<ProductCategory> productCategories;
        // interface updates
        IRepository<Product> context;
        IRepository<ProductCategory> productCategories;

        // interface's update
        public HomeController(IRepository<Product> productContext, IRepository<ProductCategory> productCategoriesContext)
        {
            context = productContext;
            productCategories = productCategoriesContext;
        }
        //Modification for product listing
        public ActionResult Index(string Category=null)
        {
            //List<Product> products = context.Collection().ToList();
            //return View(products);

            List<Product> products; // create emty list of products
            List<ProductCategory> Categories = productCategories.Collection().ToList(); // get a list of productCategories

            if(Category==null)//test to see whether the category is normal or not.
            {
                products = context.Collection().ToList();
            }
            else //Return a filtered list of products.
            {
                //IQueryable allows us to construct a filter.
                products = context.Collection().Where(p => p.Category == Category).ToList();
            }
            // Create ProductListViewModel

            ProductListViewModel model = new ProductListViewModel();
            //Sign various that I need
            model.Products = products;
            model.ProductCategories = Categories;

              return View(model);
        }

        public ActionResult Details(string Id)
        {
            Product product = context.Find(Id);
            if (product == null)
            {
                return HttpNotFound();
            }
            else
            {
                return View(product);
            }
        }
        [HttpPost]
        public ActionResult Index(HomeController r)
        {
            return View(r);
        }
            public ActionResult Change(String LanguageAbbrevation)
        {
            if (LanguageAbbrevation != null)
            {
                Thread.CurrentThread.CurrentCulture = CultureInfo.CreateSpecificCulture(LanguageAbbrevation);
                Thread.CurrentThread.CurrentUICulture = new CultureInfo(LanguageAbbrevation);
            }
            HttpCookie cookie = new HttpCookie("Language");
            cookie.Value = LanguageAbbrevation;
            Response.Cookies.Add(cookie);

            return RedirectToAction("Index", "Home");
        }
            public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}