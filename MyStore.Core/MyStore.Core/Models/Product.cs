using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyStore.Core.Models
{
    // BaseEntitiy implementation
    public class Product : BaseEntity
    {
        // BaseEntitiy has already an Id
        //public string Id { get; set; }

        [StringLength(20)]
        public string Name { get; set; }
        public string Description { get; set; }

        [Range(0, 1000)]
        public decimal Price { get; set; }
        public string Category { get; set; }
        public string Image { get; set; }

        // when I implement BaseEntity I need to get rid of the constructor because creation an Id handled in the base class

        //Constructor that we can generate Id.
        //public Product()
        //{
        //    this.Id = Guid.NewGuid().ToString();
        //}

    }

   
}
