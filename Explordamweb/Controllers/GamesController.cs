using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Explordamweb.Models;
using Explordamweb.Models.ViewModels;

namespace Explordamweb.Controllers
{
    

    public class GamesController : Controller
    {
        private IGamesRepository repository;
        public int PageSize = 21;


        public GamesController(IGamesRepository repo)
        {
            repository = repo;
        }
        

        public IActionResult List(string Games, string genre, int GamesPage = 1)
        {

            return View(new GamesListViewModel
            {
                Games = repository.Games
                .Where(p => genre == null || p.Genre == genre)
                .OrderByDescending(p => p.RecommendationCount)
                .Skip((GamesPage - 1) * PageSize)
                .Take(PageSize),
                PagingInfo = new PagingInfo
                {
                    CurrentPage = GamesPage,
                    ItemsPerPage = PageSize,
                    TotalItems = genre == null ?
                        repository.Games.Count() :
                        repository.Games.Where(e =>
                        e.Genre == genre).Count()
                },
                CurrentCategory = genre
            });
        }



        public IActionResult AltList(string Games, string Genre, string filter, string platform, int GamesPage = 1)
        {
            IEnumerable<Games> ItemList;
            int ListCount;
            IOrderedQueryable<string> AllGenres = repository.Games
            .Select(x => x.Genre)
            .Distinct()
            .OrderBy(x => x);


            if (AllGenres.Contains(Genre))
            {
                switch (platform)
                {
                    case "Windows":
                        ItemList = repository.Games.Where(g => g.Genre == Genre && g.PlatformWindows == true).OrderByDescending(g => g.Metacritic).Skip((GamesPage - 1) * PageSize).Take(PageSize);
                        ListCount = repository.Games.Where(e => e.Genre == Genre && e.PlatformWindows == true).Count();
                        break;
                    case "Linux":
                        ItemList = repository.Games.Where(g => g.Genre == Genre && g.PlatformLinux == true).OrderByDescending(g => g.Metacritic).Skip((GamesPage - 1) * PageSize).Take(PageSize);
                        ListCount = repository.Games.Where(e => e.Genre == Genre && e.PlatformLinux == true).Count();
                        break;
                    case "Mac":
                        ItemList = repository.Games.Where(g => g.Genre == Genre && g.PlatformMac == true).OrderByDescending(g => g.Metacritic).Skip((GamesPage - 1) * PageSize).Take(PageSize);
                        ListCount = repository.Games.Where(e => e.Genre == Genre && e.PlatformMac == true).Count();
                        break;
                    default:
                        ItemList = repository.Games.Where(g => g.Genre == Genre).OrderByDescending(g => g.Metacritic).Skip((GamesPage - 1) * PageSize).Take(PageSize);
                        ListCount = repository.Games.Where(e => e.Genre == Genre).Count();
                        break;
                }

                return View(new GamesListViewModel
                {
                    Games = ItemList,
                    PagingInfo = new PagingInfo
                    {
                        CurrentPage = GamesPage,
                        ItemsPerPage = PageSize,
                        TotalItems = ListCount
                    },
                    CurrentCategory = Genre
                });
            }
            else
            {
                switch (platform)
                {

                    case "Windows":
                        ItemList = repository.Games.Where(g => g.QueryName.Contains(Genre) && g.PlatformWindows == true).OrderByDescending(g => g.Metacritic).Skip((GamesPage - 1) * PageSize).Take(PageSize);
                        ListCount = repository.Games.Where(e => e.QueryName.Contains(Genre) && e.PlatformWindows == true).Count();
                        break;
                    case "Linux":
                        ItemList = repository.Games.Where(g => g.QueryName.Contains(Genre) && g.PlatformLinux == true).OrderByDescending(g => g.Metacritic).Skip((GamesPage - 1) * PageSize).Take(PageSize);
                        ListCount = repository.Games.Where(e => e.QueryName.Contains(Genre) && e.PlatformLinux == true).Count();
                        break;
                    case "Mac":
                        ItemList = repository.Games.Where(g => g.QueryName.Contains(Genre) && g.PlatformMac == true).OrderByDescending(g => g.Metacritic).Skip((GamesPage - 1) * PageSize).Take(PageSize);
                        ListCount = repository.Games.Where(e => e.QueryName.Contains(Genre) && e.PlatformMac == true).Count();
                        break;
                    default:
                        ItemList = repository.Games.Where(g => g.QueryName.Contains(Genre)).OrderByDescending(g => g.Metacritic).Skip((GamesPage - 1) * PageSize).Take(PageSize);
                        ListCount = repository.Games.Where(e => e.QueryName.Contains(Genre)).Count();
                        break;
                }

                return View(new GamesListViewModel
                {
                    Games = ItemList,
                    PagingInfo = new PagingInfo
                    {
                        CurrentPage = GamesPage,
                        ItemsPerPage = PageSize,
                        TotalItems = ListCount
                    },
                    CurrentCategory = Genre
                });
            }

        }

        public IActionResult SearchList(string searchstring)
        {
            if (searchstring == null)
            {
                return View(new GamesListViewModel
                {
                    Games = repository.Games.Where(g => g.QueryName == "Nan"),
                    PagingInfo = new PagingInfo
                    {
                        CurrentPage = 1,
                        ItemsPerPage = PageSize,
                        TotalItems = 1
                    },
                    CurrentCategory = "There was no search criterea present."
                });
            }
            else if (searchstring.Length > 3)
            {
                return View(new GamesListViewModel
                {
                    Games = repository.Games.Where(g => g.QueryName.Contains(searchstring)).OrderByDescending(g => g.RecommendationCount),
                    PagingInfo = new PagingInfo
                    {
                        CurrentPage = 1,
                        ItemsPerPage = PageSize,
                        TotalItems = 1
                    },
                    CurrentCategory = searchstring
                });
            }
            else
            {
                return View(new GamesListViewModel
                {
                    Games = repository.Games.Where(g => g.QueryName == "Nan"),
                    PagingInfo = new PagingInfo
                    {
                        CurrentPage = 1,
                        ItemsPerPage = PageSize,
                        TotalItems = 1
                    },
                    CurrentCategory = "Please type more than three characters."
                });
            }
        }

        public IActionResult SortList(string genre, string SortOn, string ascordesc, int GamesPage = 1)
        {
            IEnumerable<Games> ItemList;
            IOrderedQueryable<string> AllGenres = repository.Games
            .Select(x => x.Genre)
            .Distinct()
            .OrderBy(x => x);
            

            if (AllGenres.Contains(genre))
            {
                if (ascordesc == "Asc")
                {
                    switch (SortOn)
                    {
                        case "PriceFinal":
                            ItemList = repository.Games.Where(g => g.Genre == genre).OrderBy(g => g.PriceFinal).Skip((GamesPage - 1) * PageSize).Take(PageSize);
                            break;
                        case "QueryName":
                            ItemList = repository.Games.Where(g => g.Genre == genre).OrderBy(g => g.QueryName).Skip((GamesPage - 1) * PageSize).Take(PageSize);
                            break;
                        case "ReleaseDate":
                            ItemList = repository.Games.Where(g => g.Genre == genre).OrderBy(g => DateTime.Parse(g.ReleaseDate)).Skip((GamesPage - 1) * PageSize).Take(PageSize);
                            break;
                        default:
                            ItemList = repository.Games.Where(g => g.Genre == genre).OrderBy(g => g.Metacritic).Skip((GamesPage - 1) * PageSize).Take(PageSize);
                            break;
                    }
                }
                else
                {
                    switch (SortOn)
                    {
                        case "PriceFinal":
                            ItemList = repository.Games.Where(g => g.Genre == genre).OrderByDescending(g => g.PriceFinal).Skip((GamesPage - 1) * PageSize).Take(PageSize);
                            break;
                        case "QueryName":
                            ItemList = repository.Games.Where(g => g.Genre == genre).OrderByDescending(g => g.QueryName).Skip((GamesPage - 1) * PageSize).Take(PageSize);
                            break;
                        case "ReleaseDate":
                            ItemList = repository.Games.Where(g => g.Genre == genre).OrderByDescending(g => DateTime.Parse(g.ReleaseDate)).Skip((GamesPage - 1) * PageSize).Take(PageSize);
                            break;
                        default:
                            ItemList = repository.Games.Where(g => g.Genre == genre).OrderByDescending(g => g.Metacritic).Skip((GamesPage - 1) * PageSize).Take(PageSize);
                            break;
                    }
                }

                return View(new GamesListViewModel
                {
                    Games = ItemList,
                    PagingInfo = new PagingInfo
                    {
                        CurrentPage = GamesPage,
                        ItemsPerPage = PageSize,
                        TotalItems = genre == null ?
                            repository.Games.Count() :
                            repository.Games.Where(e =>
                            e.Genre == genre).Count()
                    },
                    CurrentCategory = genre
                });
            }
            else
            {
                if (ascordesc == "Asc")
                {
                    switch (SortOn)
                    {
                        case "PriceFinal":
                            ItemList = repository.Games.Where(g => g.QueryName.Contains(genre)).OrderBy(g => g.PriceFinal);
                            break;
                        case "QueryName":
                            ItemList = repository.Games.Where(g => g.QueryName.Contains(genre)).OrderBy(g => g.QueryName);
                            break;
                        case "ReleaseDate":
                            ItemList = repository.Games.Where(g => g.QueryName.Contains(genre)).OrderBy(g => DateTime.Parse(g.ReleaseDate));
                            break;
                        default:
                            ItemList = repository.Games.Where(g => g.QueryName.Contains(genre)).OrderBy(g => g.Metacritic);
                            break;
                    }
                }
                else
                {
                    switch (SortOn)
                    {
                        case "PriceFinal":
                            ItemList = repository.Games.Where(g => g.QueryName.Contains(genre)).OrderByDescending(g => g.PriceFinal);
                            break;
                        case "QueryName":
                            ItemList = repository.Games.Where(g => g.QueryName.Contains(genre)).OrderByDescending(g => g.QueryName);
                            break;
                        case "ReleaseDate":
                            ItemList = repository.Games.Where(g => g.QueryName.Contains(genre)).OrderByDescending(g => DateTime.Parse(g.ReleaseDate));
                            break;
                        default:
                            ItemList = repository.Games.Where(g => g.QueryName.Contains(genre)).OrderByDescending(g => g.Metacritic);
                            break;
                    }
                }
                return View(new GamesListViewModel
                {
                    Games = ItemList,
                    PagingInfo = new PagingInfo
                    {
                        CurrentPage = 1,
                        ItemsPerPage = PageSize,
                        TotalItems = genre == null ?
                         repository.Games.Count() :
                         repository.Games.Where(e =>
                         e.QueryName.Contains(genre)).Count()
                    },
                    CurrentCategory = genre
                });
            }


        }

        public IActionResult GameInfo(string Games, string Genre, string Name)
        {
            return View(new GameInfoViewModel
            {
                Game = from game in repository.Games where game.QueryName == Name select game
            });
        }

    }
}
