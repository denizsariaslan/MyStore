using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MyStore.Core.Contracts;
using MyStore.Core.Models;
using MyStore.Core.ViewModels;
using MyStore.WebUI;
using MyStore.WebUI.Controllers;

namespace MyStore.WebUI.Tests.Controllers
{
    [TestClass]
    public class HomeControllerTest
    {
        [TestMethod]
        public void Index()
        {
            //Create instances of those Mock repositories
            IRepository<Product> productContext = new Mocks.MockContext<Product>();
            IRepository<ProductCategory> productCategoryContext = new Mocks.MockContext<ProductCategory>();

            //insert a new product for testing
            productContext.Insert(new Product());
            //Controller creation
            HomeController controller = new HomeController(productContext, productCategoryContext);

            //call index method under controller to get the result

            var result = controller.Index() as ViewResult;
            var viewModel = (ProductListViewModel)result.ViewData.Model;

            //running test
            //expecting 1 product in the list of products
            Assert.AreEqual(1, viewModel.Products.Count());
        }

       
    }
}
