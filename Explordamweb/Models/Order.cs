using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace Explordamweb.Models
{
    public class Order
    {
        [BindNever]
        public int OrderID { get; set; }
        [BindNever]
        public ICollection<CartLine> Lines { get; set; }
        [BindNever]
        public bool Shipped { get; set; }
        [Required(ErrorMessage = "Please enter a name")]
        public string Name { get; set; }
        //[Required(ErrorMessage = "Please enter a surname")]
        public string Surname { get; set; }
        [Required(ErrorMessage = "Please enter an email-address")]
        public string Email { get; set; }
        //[Required(ErrorMessage = "Please enter a country name")]
        public string Country { get; set; }
        public DateTime Orderdate { get; set; }
        public string UserID { get; set; }

    }
}
