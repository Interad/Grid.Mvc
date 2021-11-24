using System.Collections.Generic;
using GridMvc.Core.Site.Models;

namespace GridMvc.Core.Site.Code
{
    public static class Report
    {
        public static IEnumerable<MonthReportModel> ListAll()
        {
            return new MonthReportModel[] {
                new MonthReportModel { Month =  "June", Amount = 101 },
                new MonthReportModel { Month = "September", Amount = 20},
                new MonthReportModel{ Month = "October", Amount = 200}
            };
        }
    }
}
