using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Explordamweb.Models
{
    public class DataVisuals
    {
        
    }

    public class ItemsPerGenre
    {
        public string Genre { get; set; }
        public int Count { get; set; }
    }

    public class SalesPerDay
    {
        public DateTime? DaySold { get; set; }
        public int OrderAmount { get; set; }
    }

    public class AccountsPerDay
    {
        public DateTime? DayMade { get; set; }
        public int AccountAmount { get; set; }
    }
    public class TurnoverPerGenre
    {
        public string Genre { get; set; }
        public double TotalProfit { get; set; }
    }
    public class TotalCashPerDay
    {
        public DateTime? Date { get; set; }
        public double Cash { get; set; }
    }
}

