using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyStore.Core.Models
{
    // Set our class to an abstract class So we're setting it as an abstract class.
    //It means we can not never create an instance of base hence to units its own.  we can only create the class that implements it.
    public abstract class BaseEntity
    {
        public string Id { get; set; }
        //when we look in our database and we want to do troubleshooting we can see exactly when particular classes were created.
        public DateTimeOffset CreatedAt { get; set; }

        // Constructor
        public BaseEntity()
        {
            //set internal Id
            this.Id = Guid.NewGuid().ToString();
            //Insert the Current datetime to the time of object creation
            this.CreatedAt = DateTime.Now;
        }
    }
}
