using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Highsoft.Web.Mvc.Charts;
using Microsoft.AspNetCore.Mvc;
using Explordamweb.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore.Extensions.Internal;

namespace Explordamweb
{
    [Authorize(Roles = "Admins")]
    public partial class ChartsController : Controller
    {
        private IGamesRepository repository;
        private IOrderRepository orepository;
        private IUserRepository urepository;


        public ChartsController(IGamesRepository repo, IOrderRepository orepo, IUserRepository urepo)
        {
            repository = repo;
            orepository = orepo;
            urepository = urepo;

        }


        public List<ItemsPerGenre> CountGamesPerGenre()
        {
            List<ItemsPerGenre> itemspergenre = new List<ItemsPerGenre>();

            foreach (var g in repository.Games
            .Select(x => x.Genre)
            .Distinct()
            .OrderBy(x => x))
            {
                itemspergenre.Add(new ItemsPerGenre
                {
                    Genre = g,
                    Count = repository.Games.Where(x => x.Genre == g).Count()


                });
            }
            return itemspergenre;


        }

        public List<TurnoverPerGenre> TurnoverPerGenre()
        {

            List<TurnoverPerGenre> turnoverPerGenre = new List<TurnoverPerGenre>();
            List<int> ls = new List<int>();
            foreach (var order in orepository.Orders)
            {
                foreach (var cartline in order.Lines)
                {
                    for (int i = 0; i < cartline.Quantity; i++)
                    {
                        ls.Add(cartline.Game.ID);
                    }
                }
            }
            var GenreTotalCost =
                from Game in repository.Games
                where ls.Contains(Game.ID)
                group Game by Game.Genre into GameGroup
                select new
                {
                    Genre = GameGroup.Key,
                    TotalProfit = GameGroup.Sum(x => x.PriceFinal),
                };
            foreach (var s in GenreTotalCost)
            {
                turnoverPerGenre.Add(new TurnoverPerGenre {
                    Genre = s.Genre,
                    TotalProfit = Convert.ToDouble(s.TotalProfit) }); 
         
            }

            return turnoverPerGenre;

        }


        public List<SalesPerDay> CountSalesPerDay()
        {
            List<SalesPerDay> salesPerDay = new List<SalesPerDay>();

            foreach (var g in orepository.Orders
            .Select(x => x.Orderdate.Date)
            .Distinct()
            .OrderBy(x => x))
            {
                salesPerDay.Add(new SalesPerDay
                {
                    DaySold = g,
                    OrderAmount = orepository.Orders.Where(x => x.Orderdate.Date == g).Count()


                });
            }

            return salesPerDay;


        }

        public List<TotalCashPerDay> TurnoverPerDay()
        {
            List<TotalCashPerDay> turnoverPerDay = new List<TotalCashPerDay>();

            foreach (var i in orepository.Orders.Where(l => l.OrderID >0).Select(x => x.Orderdate.Date).Distinct().OrderBy(x => x))
            {
                var orders = orepository.Orders.Where(x => x.Orderdate.Date == i);
                decimal TotalOfThisDay = 0;

                foreach (var order in orders)
                {
                    foreach (var product in order.Lines)
                    {
                        TotalOfThisDay += product.Game.PriceFinal;
                    }
                }

                turnoverPerDay.Add(new TotalCashPerDay
                {
                    Date = i,
                    Cash = Convert.ToDouble(TotalOfThisDay)
                });
            }



            return turnoverPerDay;
        }

        public List<AccountsPerDay> CountAccountsPerDay()
        {
            List<AccountsPerDay> accountsPerDay = new List<AccountsPerDay>();

            foreach (var g in urepository.AppUsers
            .Select(x => x.CreatedDate.Date)
            .Distinct()
            .OrderBy(x => x))
            {
                accountsPerDay.Add(new AccountsPerDay
                {
                    DayMade = g,
                    AccountAmount = urepository.AppUsers.Where(x => x.CreatedDate.Date == g).Count()


                });
            }

            return accountsPerDay;


        }


        public ActionResult BarBasicTurnover()
        {
            var turnoverPerGenre = TurnoverPerGenre();
            

            List<ColumnSeriesData> TurnoverGenreData = new List<ColumnSeriesData>();
            List<String> TurnoverGenreCats = new List<String>();

            foreach (var s in turnoverPerGenre)
            {
                TurnoverGenreData.Add(new ColumnSeriesData { Y = s.TotalProfit });
                TurnoverGenreCats.Add(s.Genre);
            }
 
 

            ViewData["turnoverGenreData"] = TurnoverGenreData;
            ViewData["turnoverGenreCats"] = TurnoverGenreCats;


            return View();
        }


        public ActionResult PieBasic()
        {
            List<PieSeriesData> pieData = new List<PieSeriesData>();
            var gamesPerGenre = CountGamesPerGenre();
            foreach (var g in gamesPerGenre)
            {
                pieData.Add(new PieSeriesData { Name = g.Genre, Y = g.Count });

            }

            ViewData["pieData"] = pieData;

            return View();
        }


        public ActionResult LineBasicSales()
        {
            var salesTotal = CountSalesPerDay();

            List<LineSeriesData> salesData = new List<LineSeriesData>();
            List<String> salesDataTime = new List<String>();

            foreach (var g in salesTotal)
            {
                salesData.Add(new LineSeriesData { Y = g.OrderAmount});
                salesDataTime.Add(g.DaySold.Value.ToShortDateString());

            }

            //salesTotal.ForEach(p => salesData.Add(new LineSeriesData { Y = p }));

            ViewData["salesData"] = salesData;
            ViewData["salesDataTime"] = salesDataTime;


            return View();
        }

        public ActionResult LineBasicTurnover()
        {
            var turnoverTotal = TurnoverPerDay();

            List<LineSeriesData> turnoverData = new List<LineSeriesData>();
            List<String> turnoverDataTime = new List<String>();

            foreach (var g in turnoverTotal)
            {
                turnoverData.Add(new LineSeriesData { Y = g.Cash });
                turnoverDataTime.Add(g.Date.Value.ToShortDateString());

            }

            ViewData["turnoverData"] = turnoverData;
            ViewData["turnoverDataTime"] = turnoverDataTime;


            return View();
        }





        public ActionResult LineBasicAccounts()
        {
            var accountsTotal = CountAccountsPerDay();

            List<LineSeriesData> accountsData = new List<LineSeriesData>();
            List<String> accountsDataTime = new List<String>();

            foreach (var g in accountsTotal)
            {
                accountsData.Add(new LineSeriesData { Y = g.AccountAmount });
                accountsDataTime.Add(g.DayMade.Value.ToShortDateString());

            }

            ViewData["accountsData"] = accountsData;
            ViewData["accountsDataTime"] = accountsDataTime;


            return View();
        }

    }
}