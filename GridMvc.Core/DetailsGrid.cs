using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Http;

namespace GridMvc.Core
{
    
    public class DetailsGrid<TKey, TValue>:  Grid<TValue> where TKey : class
                                                         where TValue : class
                                                         

    {
        private System.Func<TKey, IEnumerable<TValue>> DetailsDataSource;

        public DetailsGrid(Func<TKey, IEnumerable<TValue>> sourceFunc, HttpContext context) :base(null, context)
        {
            this.DetailsDataSource = sourceFunc;
        }

        public void RetreiveDataByKey(TKey key)
        {
            EnsureLastData();
            BeforeItems = this.DetailsDataSource(key).AsQueryable();
        }

        public bool IsDetailsGrid {
            get { return true; }
        }
    }
    
}
