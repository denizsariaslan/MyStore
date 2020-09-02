using MyStore.Core.Contracts;
using MyStore.Core.Models;
using MyStore.Core.ViewModels;
using MyStore.DataAccess.InMemory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MyStore.WebUI.Controllers
{
    public class ProductManagerController : Controller
    {

        //    ProductRepository context;
        //    ProductCategoryRepository productCategories;

        //these two lines updated when generics implemented
        ////InMemoryRepository<Product> context;
        ////InMemoryRepository<ProductCategory> productCategories;
        // interface updates
        IRepository<Product> context;
        IRepository<ProductCategory> productCategories;

        // interface update
        public ProductManagerController(IRepository<Product> productContext, IRepository<ProductCategory> productCategoriesContext )
        {
            context = productContext;
            productCategories = productCategoriesContext;
        }
        // GET: ProductManager
        public ActionResult Index()
        {
            List<Product> products = context.Collection().ToList();
            return View(products);
        }

        public ActionResult Create()
        {
            //created reference as viewModel
            ProductManagerViewModel viewModel = new ProductManagerViewModel();

            //Put empty product
            viewModel.Product = new Product();
            //send in this product categories which I will get grom database
            viewModel.ProductCategories = productCategories.Collection();
            // return to page this view instead of product
            return View(viewModel);
        }

        [HttpPost]
        public ActionResult Create(Product product)
        {
            if (!ModelState.IsValid)
            {
                return View(product);
            }
            else
            {
                context.Insert(product);
                context.Commit();

                return RedirectToAction("Index");
            }

        }

        public ActionResult Edit(string Id)
        {
            Product product = context.Find(Id);
            if (product == null)
            {
                return HttpNotFound();
            }
            else
            {
                //    So just to recap what we're doing is getting either an empty product or the product with loaded from
                //    the database who return it to the view and instead of returning that product to the view we're instead
                //    using the View model to hold that products and a list of all the product categories out of our database
                //    but because we're now passing a different view model.
                //    That's the view we need to actually update the view pitches themselves.
                //    open the product manager view pages and update model  @model MyStore.Core.Model.Product  AS 
                //     @model MyStore.Core.ViewModels.ProductManagerViewModel

                //created reference as viewModel
                ProductManagerViewModel viewModel = new ProductManagerViewModel();

                //Put empty product
                viewModel.Product = new Product();
                //send in this product categories which I will get grom database
                viewModel.ProductCategories = productCategories.Collection();
                // return to page this view instead of product
                return View(viewModel);
            }
        }

        [HttpPost]
        public ActionResult Edit(Product product, string Id)
        {
            Product productToEdit = context.Find(Id);

            if (productToEdit == null)
            {
                return HttpNotFound();
            }
            else
            {
                if (!ModelState.IsValid)
                {
                    return View(product);
                }

                productToEdit.Category = product.Category;
                productToEdit.Description = product.Description;
                productToEdit.Image = product.Image;
                productToEdit.Name = product.Name;
                productToEdit.Price = product.Price;

                context.Commit();

                return RedirectToAction("Index");
            }
        }

        public ActionResult Delete(string Id)
        {
            Product productToDelete = context.Find(Id);

            if (productToDelete == null)
            {
                return HttpNotFound();
            }
            else
            {
                return View(productToDelete);
            }
        }

        [HttpPost]
        [ActionName("Delete")]
        public ActionResult ConfirmDelete(string Id)
        {
            Product productToDelete = context.Find(Id);

            if (productToDelete == null)
            {
                return HttpNotFound();
            }
            else
            {
                context.Delete(Id);
                context.Commit();
                return RedirectToAction("Index");
            }
        }
    }
}