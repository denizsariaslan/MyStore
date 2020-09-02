using MyStore.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Caching;
using System.Text;
using System.Threading.Tasks;

namespace MyStore.DataAccess.InMemory
{
    // T would be a base entitiy(whenever we pass in an object it must be the type base entity or at least inherit from base entity.
    //Because base entitiy has an Id It means when ever we reference Id our genneric class knows what that is.
    public class InMemoryRepository<T> where T : BaseEntity
    {
        //Create ObjectCache
        ObjectCache cache = MemoryCache.Default;
        //Create internal list referencing our place holder "<T>" .we are not referencing as list of product or productcategory
        List<T> items;
        //adding extra internal variable that is really just to give us an easy handle to set how we're going to store our objects in the cache.
        //Because every time we create cash so we'd have a separate cash for products we need to build to tell it what that needs going to be
        //because it would need to be different each time.
        string className;

        //Creeate Contsructor that initialize our repository
        public InMemoryRepository()
        {
            // Reflection on className
            className = typeof(T).Name; // getting our actual name of our class which are Product.cs or ProductCategory.cs
            // Initialize our internal item class. 
            items = cache[className] as List<T>;
            //Which as before we check to see if we any at items in our cache.
            if (items == null)
            {
                items = new List<T>();
            }

        }
        //Creating generic Commit function(method) whict will simply store our items in memory.
        public void Commit()
        {
            cache[className] = items;

        }

        //Creating standart method As Insert, Update, Delete but end point not going to end with product and product category what we did before.
        //instead of using product or product category we'll be storing <T>

        // Insert Method

        public void Insert(T t)
        {
            items.Add(t);
        }
        // Update Method 

        public void Update(T t)
        {
            T tToUpdate = items.Find(i => i.Id == t.Id);

            if(tToUpdate != null)
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
            if(t != null)
            {
                return t;
            }
            else
            {
                throw new Exception(t + " Not found");
            }


        }

        //Collection Method

            public IQueryable<T> Collection()
        {
            return items.AsQueryable();
        }
        public void Delete(string Id)
        {
            T tToDelete = items.Find(i => i.Id == Id);

            if(tToDelete != null)
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
