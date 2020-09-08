using MyStore.Core.Contracts;
using MyStore.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
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
        public ActionResult Index()
        {
            List<Product> products = context.Collection().ToList();
            return View(products);
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