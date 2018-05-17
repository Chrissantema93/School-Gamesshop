using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using System.Threading.Tasks;

namespace Explordamweb.Models
{
    public static class IdentitySeedData
    {
        private const string adminUser = "**classified**";
        private const string adminPassword = "**classified**";



        public static async Task EnsurePopulated(UserManager<AppUser> userManager)
        {
           
            AppUser user = await userManager.FindByIdAsync(adminUser);
            if (user == null)
            {
                user = new AppUser("**classified**");
                await userManager.CreateAsync(user, adminPassword);
            }
        }
    }
}