using MyStore.Core.Contracts;
using MyStore.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyStore.WebUI.Tests.Mocks
{
    public class MockContext<Products> :IRepository<Products> where Products : BaseEntity
    {

        List<Products> items;
        string className;

       // public InMemoryRepository()
        public MockContext()
        {
            items = new List<Products>();
            //className = typeof(Products).Name; 
            //items = cache[className] as List<Products>; 
            //if (items == null)
            //{
            //    items = new List<Products>();
            //}
        }

        public void Commit()
        {
            // cache[className] = items;
            return;
        }

        // Insert Method

        public void Insert(Products t)
        {
            items.Add(t);
        }
        // Update Method 

        public void Update(Products t)
        {
            Products tToUpdate = items.Find(i => i.Id == t.Id);

            if (tToUpdate != null)
            {
                tToUpdate = t;
            }
            else
            {
                throw new Exception(className + " Not Found");
            }
        }

        //Find Method
        public Products Find(string Id)
        {
            Products t = items.Find(i => i.Id == Id);
            if (t != null)
            {
                return t;
            }
            else
            {
                throw new Exception(t + " Not found");
            }
        }
        public IQueryable<Products> Collection()
        {
            return items.AsQueryable();
        }
        public void Delete(string Id)
        {
            Products tToDelete = items.Find(i => i.Id == Id);

            if (tToDelete != null)
            {
                items.Remove(tToDelete);
            }
            else
            {
                throw new Exception(className + " Not Found");
            }
        }

        public Products Search(string Name)
        {
            return Find(Name);
        }

    }
}
