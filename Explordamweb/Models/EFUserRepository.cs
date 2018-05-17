using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Explordamweb.Models
{
    public class EFUserRepository : IUserRepository
    {
        private AppIdentityDbContext context;
        public EFUserRepository(AppIdentityDbContext ctx)
        {
            context = ctx;
        }

        public IQueryable<AppUser> AppUsers => context.Users;
    }
}
