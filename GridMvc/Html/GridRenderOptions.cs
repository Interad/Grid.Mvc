using System;

namespace GridMvc.Html
{
    public class GridRenderOptions
    {
        public GridRenderOptions(string gridName, string viewName)
        {
            ViewName = viewName;
            GridName = gridName;
            Selectable = true;
            AllowMultipleFilters = false;
            DisplayActiveFilter = false;
        }

        public GridRenderOptions()
            : this(string.Empty, GridExtensions.DefaultPartialViewName)
        {
        }

        /// <summary>
        ///     Specify partial view name for render grid
        /// </summary>
        public string ViewName { get; set; }

        /// <summary>
        ///     Is multiple filters allowed
        /// </summary>
        public bool AllowMultipleFilters { get; set; }

        /// <summary>
        ///     Show active filter in table header
        /// </summary>
        public bool DisplayActiveFilter { get; set; }

        /// <summary>
        ///     Gets or set grid items selectable
        /// </summary>
        public bool Selectable { get; set; }

        /// <summary>
        ///     Specify grid Id on the client side
        /// </summary>
        public string GridName { get; set; }

        /// <summary>
        ///     Specify to render grid body only
        /// </summary>
        public bool RenderRowsOnly { get; set; }

        /// <summary>
        ///     Specify to show a grid items count
        /// </summary>
        public bool ShowGridItemsCount { get; set; }

        /// <summary>
        ///     To show string while show grid items count
        /// </summary>
        [Obsolete("Use GridCountFormatString or IGrid.RenderGridCount() instead.")]
        public string GridCountDisplayName
        {
            get => GridCountFormatString;
            set => GridCountFormatString = value + ": {0}";
        }

        /// <summary>
        ///     Format the grid items count display string using a format string.<br/>
        ///     There is one positional parameter {0} where the actual count is displayed.<br/>
        ///     Default is "Items count: {0}"
        /// </summary>
        public string GridCountFormatString { get; set; }

        public static GridRenderOptions Create(string gridName)
        {
            return new GridRenderOptions(gridName, GridExtensions.DefaultPartialViewName);
        }

        public static GridRenderOptions Create(string gridName, string viewName)
        {
            return new GridRenderOptions(gridName, viewName);
        }
    }
}