using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyStore.Core.Models
{
    public class Customer : BaseEntity
    {
        public string UserId { get; set; } // I will link our customer to our underlying log in information
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; } //Store the Email here. Technically I should need to do Email here because I could get that from the log in information, but sometimes it is simply easier to duplicate data when I am trying to retrieve the information later own.
        public string Street { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Country { get; set; }       
        public string ZipCode { get; set; }
    }
}
