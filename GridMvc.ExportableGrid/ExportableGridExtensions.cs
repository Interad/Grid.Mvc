using GridMvc.Html;
using GridMvc.Utility.ExportableGrid;
using System.Web.Mvc;

namespace GridMvc.ExportableGrid
{
    public static class ExportableGridExtensions
    {
        public static HtmlGrid<T> Grid<T>(this HtmlHelper helper, ExportableGrid<T> grid) where T : class
        {
            return grid.AsHtmlGrid(helper);
        }
    }
}
