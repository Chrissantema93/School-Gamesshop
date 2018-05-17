using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Explordamweb.Models
{
    public interface IOrderRepository
    {
        IQueryable<Order> Orders { get; }
        void SaveOrder(Order order);
        void EditStatus(Order order);
    }
}
