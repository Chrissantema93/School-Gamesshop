using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Explordamweb.Models;
using Explordamweb.Models.ViewModels;
using Explordamweb.Controllers;

namespace Explordamweb.Views.Shared.Components
{

    public class Layout : PageModel
    {
        [BindProperty]
        public string Genre { get; set; }

        public void OnGet()
        {

        }

    }
}
