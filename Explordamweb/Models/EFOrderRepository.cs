using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace Explordamweb.Models
{
    public class EFOrderRepository : IOrderRepository
    {
        private ApplicationDbContext context;
        public EFOrderRepository(ApplicationDbContext ctx)
        {
            context = ctx;
        }
        public IQueryable<Order> Orders => context.Orders
       .Include(o => o.Lines)
       .ThenInclude(l => l.Game);

        public void SaveOrder(Order order)
        {
            context.AttachRange(order.Lines.Select(l => l.Game));
            if (order.OrderID == 0)
            {
                context.Orders.Add(order);
            }
            context.SaveChanges();
        }

        public void EditStatus(Order order)
        {
            order.Shipped = true;
            context.SaveChanges();
        }
    }
}
