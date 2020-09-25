using MyStore.Core.Contracts;
using MyStore.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyStore.WebUI.Tests.Mocks
{
    public class MockContext<T> :IRepository<T> where T: BaseEntity
    {

        List<T> items;
        string className;

       // public InMemoryRepository()
        public MockContext()
        {
            items = new List<T>();   
            //className = typeof(T).Name; 
            //items = cache[className] as List<T>; 
            //if (items == null)
            //{
            //    items = new List<T>();
            //}
        }
      
        public void Commit()
        {
            // cache[className] = items;
            return;
        }

        // Insert Method

        public void Insert(T t)
        {
            items.Add(t);
        }
        // Update Method 

        public void Update(T t)
        {
            T tToUpdate = items.Find(i => i.Id == t.Id);

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
        public T Find(string Id)
        {
            T t = items.Find(i => i.Id == Id);
            if (t != null)
            {
                return t;
            }
            else
            {
                throw new Exception(t + " Not found");
            }
        }
        public IQueryable<T> Collection()
        {
            return items.AsQueryable();
        }
        public void Delete(string Id)
        {
            T tToDelete = items.Find(i => i.Id == Id);

            if (tToDelete != null)
            {
                items.Remove(tToDelete);
            }
            else
            {
                throw new Exception(className + " Not Found");
            }
        }

    }
}
