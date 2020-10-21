using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyStore.Core.Models
{
    // BaseEntitiy implementation
    public class ProductCategory : BaseEntity
    {
        // BaseEntitiy has already an Id
        //public string Id { get; set; }
     
        public string Category { get; set; }

        // when I implement BaseEntity I need to get rid of the constructor because creation an Id handled in the base class
        ////Constractor that we can generate Id.
        ////public ProductCategory()
        ////{
        ////    this.Id = Guid.NewGuid().ToString();
        ////}
    }
}
