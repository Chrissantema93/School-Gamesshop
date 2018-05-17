using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.DependencyInjection;

namespace Explordamweb.Models
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        { }
        public DbSet<Games> Games { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<Wishitem> WishList { get; set; }
        //public DbSet<Users> Users { get; set; }
       
    }
}
