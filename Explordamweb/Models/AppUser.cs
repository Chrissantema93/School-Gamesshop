using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;

namespace Explordamweb.Models
{
    public enum Countries
    {
        None, Netherlands, Belgium, England
    }

    public enum AccountTypes
    {
        Basic, Premium
    }


    public class AppUser : IdentityUser
    {
        
        public Countries Country { get; set; }
        public AccountTypes AccountType { get; set; }
        public DateTime CreatedDate { get; set; }
        public AppUser() { }
       

        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<AppUser> manager)
        {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType

            var userIdentity = new ClaimsIdentity(await manager.GetClaimsAsync(this));

            // Add custom user claims here
            userIdentity.AddClaim(new Claim("UserName", UserName));
            userIdentity.AddClaim(new Claim("Email", Email));

            return userIdentity;
        }


        public AppUser(string userName) : base(userName)
        { }

        public AppUser(string userName, DateTime createdDate) : base(userName)
        { }
    }
}
