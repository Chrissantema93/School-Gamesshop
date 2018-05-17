using System.Collections.Generic;
using System.Linq;

namespace Explordamweb.Models
{
    public interface IGamesRepository
    {
        IQueryable<Games> Games { get; }

        void SaveProduct(Games game);

        Games DeleteGame(Games game);
    }
}