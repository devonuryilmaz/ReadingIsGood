using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReadingIsGood.Core.Models.Responses
{
    public class StatisticMonthlyReportModel
    {
        public int Month { get; set; }
        public int TotalOrderCount { get; set; }
        public int TotalBookCount { get; set; }
        public decimal TotalPurchasedAmount { get; set; }
    }
}
