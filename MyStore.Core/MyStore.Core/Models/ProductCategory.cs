using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyStore.Core.Models
{
    public class ProductCategory
    {
        public string Id { get; set; }
        public string Category { get; set; }

        //Constractor that we can generate Id.
        public ProductCategory()
        {
            this.Id = Guid.NewGuid().ToString();
        }
    }
}
