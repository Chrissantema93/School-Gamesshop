using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace Explordamweb.Models
{
    public class EFWishListRepository : IWishListRepository
    {
        private ApplicationDbContext context;
        public EFWishListRepository(ApplicationDbContext ctx)
        {
            context = ctx;
            
        }

        IQueryable<Wishitem> IWishListRepository.WishList => context.WishList;

        public void AddItem(Wishitem item)
        {
            if (context.WishList.Where(w => w.GameId == item.GameId && w.UserId == item.UserId).FirstOrDefault() == null)
            {
                context.WishList.Add(item);
                context.SaveChanges();
            }
        }

        public void RemoveItem(Wishitem item)
        {
            context.WishList.Remove(item);
            context.SaveChanges();
        }
    }
}

