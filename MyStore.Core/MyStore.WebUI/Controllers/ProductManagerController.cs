using MyStore.Core.Contracts;
using MyStore.Core.Models;
using MyStore.Core.ViewModels;
using MyStore.DataAccess.InMemory;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MyStore.WebUI.Controllers
{
   // [Authorize(Roles = "Admin")] //Authorization for Admin
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

        // interface's update
        public ProductManagerController(IRepository<Product> productContext, IRepository<ProductCategory> productCategoriesContext)
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

        [HttpPost]
     
        public ActionResult Search(string searchtext)
        {
           List<Product> product = context.Collection().Where(p=>p.Name.Contains(searchtext) || p.Category.Contains(searchtext) || p.Description.Contains(searchtext)).ToList();
            if (product == null)
            {
                return HttpNotFound();
            }
            else
            {
                  
                return View("Index", product);
            }
        }

        public ActionResult Create()
        {
            //created reference as viewModel
            ProductManagerViewModel viewModel = new ProductManagerViewModel();

            //Put empty product
            viewModel.Product = new Product();
            //send in this product categories which I will get from database
            viewModel.ProductCategories = productCategories.Collection();
            // return to page this view instead of product
            return View(viewModel);
        }

        [HttpPost]
        // upload images modification
        public ActionResult Create(Product product, HttpPostedFileBase file)
        {
            if (!ModelState.IsValid)
            {
                return View(product);
            }
            else
            {
                if (file != null) //check the file make sure actually exist because it's possible they would save or create could have without an image
                {
                    // setting ipage property = on the product itself going to give it a name as id it will always have a unique file reference +  find out the current extencion
                    product.Image = product.Id + Path.GetExtension(file.FileName);
                    //Saving to the disk.
                    string path = Path.Combine(Server.MapPath("//Content//ProductImages//") + product.Image);
                    file.SaveAs(path);
                }
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

                //Call exciting product
                viewModel.Product = product;
                //send in this product categories which I will get from database
                viewModel.ProductCategories = productCategories.Collection();
                // return to page this view instead of product
                return View(viewModel);
            }
        }

        [HttpPost]
        // upload images modification
        public ActionResult Edit(Product product, string Id, HttpPostedFileBase file)
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
                if(file != null)
                {
                    productToEdit.Image = product.Id + Path.GetExtension(file.FileName);
                    file.SaveAs(Server.MapPath("//Content//ProductImages//") + productToEdit.Image);

                }
                productToEdit.Category = product.Category;
                productToEdit.Description = product.Description;
                //productToEdit.Image = product.Image;
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