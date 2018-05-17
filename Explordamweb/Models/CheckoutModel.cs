using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Explordamweb.Models
{
    public class CheckoutModel
    {
        public Order Order { get; set; }
        public AppUser AppUser { get; set; }
    }
}
