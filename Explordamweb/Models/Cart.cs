using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Explordamweb.Models
{
    public class Cart
    {
   
        private List<CartLine> lineCollection = new List<CartLine>();
        public virtual void AddItem(Games game, int quantity)
        {
            CartLine line = lineCollection
            .Where(p => p.Game.ID == game.ID)
            .FirstOrDefault();
            if (line == null)
            {
                lineCollection.Add(new CartLine
                {
                    Game = game,
                    Quantity = quantity
                });
            
            }
            else
            {
                line.Quantity += quantity;
            }
        }
        public virtual void RemoveLine(Games game) => lineCollection.RemoveAll(l => l.Game.ID == game.ID);
        public virtual decimal ComputeTotalValue() => lineCollection.Sum(e => Convert.ToDecimal(e.Game.PriceFinal) * e.Quantity);
        public virtual void Clear() => lineCollection.Clear();
        public virtual IEnumerable<CartLine> Lines => lineCollection;
    }
    public class CartLine
    {
        public int CartLineID { get; set; }
        public Games Game { get; set; }
        public int Quantity { get; set; }
    }
    
}