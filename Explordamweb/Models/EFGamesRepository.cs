using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Explordamweb.Models
{
    public class EFGamesRepository : IGamesRepository
    {
        private ApplicationDbContext context;
        public EFGamesRepository(ApplicationDbContext ctx)
        {
            context = ctx;
        }

        public IQueryable<Games> Games => context.Games;

        public void SaveProduct(Games game)
        {
            if (game.ID == 0)
            {
                context.Games.Add(game);
            }
            else
            {
                Games dbEntry = context.Games
                .FirstOrDefault(p => p.ID == game.ID);
                if (dbEntry != null)
                {
                    dbEntry.ID = game.ID;
                    dbEntry.QueryName = game.QueryName;
                    dbEntry.PriceFinal = game.PriceFinal / 100;
                    dbEntry.Genre = game.Genre;
                }
            }
            context.SaveChanges();
        }

        public Games DeleteGame(Games game)
        {
            Games dbEntry = context.Games
            .FirstOrDefault(p => p.ID == game.ID);
            if (dbEntry != null)
            {
                context.Games.Remove(dbEntry);
                context.SaveChanges();
            }
            return dbEntry;
        }
    }
}


