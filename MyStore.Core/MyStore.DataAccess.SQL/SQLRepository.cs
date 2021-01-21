using MyStore.Core.Contracts;
using MyStore.Core.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace MyStore.DataAccess.SQL
{
    public class SQLRepository<Products> : IRepository<Products> where Products : BaseEntity
    {
        //Internal varialbes
        internal DataContext context;
        internal DbSet<Products> dbSet;

        //Constructor
        //Constructor needs to allow us to passing in a date context.
        //I need to do is set the underlying table. I do that by referencing the context and calling the set command passing in
        //Model that we are wanting to work against In this case is "T"

        public SQLRepository(DataContext context){
            this.context = context; // initialize context object
            this.dbSet = context.Set<Products>();  // Set underlying table from model T

        }

        public IQueryable<Products> Collection()
        {
            return dbSet;
        }

        public void Commit()
        {
            context.SaveChanges();
        }

        public void Delete(string Id)
        {
            var t = Find(Id); //first thing to do is find the object based on its Id which I can use the internal Find method for that which is created.
            if (context.Entry(t).State == EntityState.Detached) //I need to check the state of the entry.
                dbSet.Attach(t);

            dbSet.Remove(t);
        }

        public Products Find(string Id)
        {
            return dbSet.Find(Id);
        }

        public Products Search(string Name)
        {
            return dbSet.Find(Name);
        }

        public void Insert(Products t)
        {
            dbSet.Add(t);
        }

        public void Update(Products t)
        {
            dbSet.Attach(t);
            context.Entry(t).State = EntityState.Modified; //This tells Entity framework that when we call the save change this method to look for this object which is t and persists to the database
        }
    }
}
