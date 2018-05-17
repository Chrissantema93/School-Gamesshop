using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.ComponentModel.DataAnnotations.Schema;

namespace Explordamweb.Models
{
    public class Wishitem
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string UserId { get; set; }
        public string UserName { get; set; }
        public int GameId { get; set; }
        public string GameImage { get; set; }
        public string GameName { get; set; }
        public string GameGenre { get; set; }
        public decimal GamePrice { get; set; }
        public DateTime DateAdded { get; set; }
        
    }
}
