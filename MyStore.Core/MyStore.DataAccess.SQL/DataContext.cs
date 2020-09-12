using MyStore.Core.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyStore.DataAccess.SQL
{
    //This class is going to inherit from a data into framework class called db context.
    // DbContext has all the base methods and actions we need to throughout data context to work
    public class DataContext : DbContext
    {
        //Created a constructor so that we can capture and passing that connection string that the base class is expecting.
        public DataContext()
            //ConnectionStrings's Name is DefaultConnection in Web.config
            : base("DefaultConnection")  {

        }

        public DbSet<Product> Products { get; set; }
        public DbSet<ProductCategory> ProductCategories { get; set; }
        
        public DbSet<Cart> Carts { get; set; } //Setting Cart model in to database
        public DbSet<CartItem> CartItems { get; set; } //Setting CartItem model in to database
    }
}
