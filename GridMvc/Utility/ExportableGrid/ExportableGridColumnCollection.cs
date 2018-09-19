using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace GridMvc.Utility.ExportableGrid
{
    public class ExportableGridColumnCollection<T> : List<ExportableGridColumn<T>> where T : class
    {
        public ExportableGridColumn<T> Add()
        {
            var column = new ExportableGridColumn<T>();
            Add(column);
            return column;
        }

        public ExportableGridColumn<T> Add(Expression<Func<T, object>> value)
        {
            var column = new ExportableGridColumn<T>(value);
            Add(column);
            return column;
        }
    }
}
