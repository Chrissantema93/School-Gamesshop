using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Explordamweb.Models
{
    public interface IWishListRepository
    {
        IQueryable<Wishitem> WishList { get; }

        void AddItem(Wishitem item);

        void RemoveItem(Wishitem item);
    }
}
