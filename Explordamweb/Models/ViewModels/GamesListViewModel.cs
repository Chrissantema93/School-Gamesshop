using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Explordamweb.Models;

namespace Explordamweb.Models.ViewModels
{ 
    public class GamesListViewModel
    {
        public IEnumerable<Games> Games { get; set; }
        public PagingInfo PagingInfo { get; set; }
        public string CurrentCategory { get; set; }

    }
}
