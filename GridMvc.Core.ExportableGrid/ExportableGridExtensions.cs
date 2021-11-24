using GridMvc.Core.Html;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace GridMvc.Core.ExportableGrid
{
    public static class ExportableGridExtensions
    {
        public static HtmlGrid<T> Grid<T>(this IHtmlHelper helper, ExportableGrid<T> grid) where T : class
        {
            return grid.AsHtmlGrid(helper);
        }
    }
}
