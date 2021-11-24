using System.Linq;

namespace GridMvc.Core.Filtering
{
    public interface IColumnFilter<T>
    {
        IQueryable<T> ApplyFilter(IQueryable<T> items, ColumnFilterValue value);
    }
}