using System;
using System.IO;
using System.Web.Mvc;
using GridMvc.Columns;
using GridMvc.Pagination;

namespace GridMvc.Html
{
    /// <summary>
    ///     Grid adapter for html helper
    /// </summary>
    public class GridHtmlOptions<T> : IGridHtmlOptions<T> where T : class
    {
        private readonly Grid<T> _source;
        private readonly ViewContext _viewContext;

        public GridHtmlOptions(Grid<T> source, ViewContext viewContext, string viewName)
        {
            _source = source;
            _viewContext = viewContext;
            GridViewName = viewName;
        }

        public string GridViewName { get; set; }

        #region IGridHtmlOptions<T> Members

        public string ToHtmlString()
        {
            return RenderPartialViewToString(GridViewName, this, _viewContext);
        }

        public string Render()
        {
            return ToHtmlString();
        }

        public IGridHtmlOptions<T> Columns(Action<IGridColumnCollection<T>> columnBuilder)
        {
            columnBuilder(_source.Columns);
            return this;
        }

        public IGridHtmlOptions<T> WithPaging(int pageSize)
        {
            return WithPaging(pageSize, 0);
        }

        public IGridHtmlOptions<T> WithPaging(int pageSize, int maxDisplayedItems)
        {
            return WithPaging(pageSize, maxDisplayedItems, string.Empty);
        }

        public IGridHtmlOptions<T> WithPaging(int pageSize, int maxDisplayedItems, string queryStringParameterName)
        {
            return WithPaging(pageSize, maxDisplayedItems, queryStringParameterName, null);
        }

        public IGridHtmlOptions<T> WithPaging(int pageSize, int maxDisplayedItems, string queryStringParameterName, int? itemsCountOverwrite)
        {
            _source.EnablePaging = true;
            _source.Pager.PageSize = pageSize;

            if (itemsCountOverwrite.HasValue)
                _source.Pager.ItemsCountOverwrite = itemsCountOverwrite.Value;

            var pager = _source.Pager as GridPager; //This setting can be applied only to default grid pager
            if (pager == null) return this;

            if (maxDisplayedItems > 0)
                pager.MaxDisplayedPages = maxDisplayedItems;
            if (!string.IsNullOrEmpty(queryStringParameterName))
                pager.ParameterName = queryStringParameterName;
            _source.Pager = pager;
            return this;
        }

        public IGridHtmlOptions<T> Sortable()
        {
            return Sortable(true);
        }

        public IGridHtmlOptions<T> Sortable(bool enable)
        {
            _source.DefaultSortEnabled = enable;
            foreach (IGridColumn column in _source.Columns)
            {
                var typedColumn = column as IGridColumn<T>;
                if (typedColumn == null) continue;
                typedColumn.Sortable(enable);
            }
            return this;
        }

        public IGridHtmlOptions<T> Filterable()
        {
            return Filterable(true);
        }

        public IGridHtmlOptions<T> Filterable(bool enable)
        {
            _source.DefaultFilteringEnabled = enable;
            foreach (IGridColumn column in _source.Columns)
            {
                var typedColumn = column as IGridColumn<T>;
                if (typedColumn == null) continue;
                typedColumn.Filterable(enable);
            }
            return this;
        }

        public IGridHtmlOptions<T> Selectable(bool set)
        {
            _source.RenderOptions.Selectable = set;
            return this;
        }

        public IGridHtmlOptions<T> EmptyText(string text)
        {
            _source.EmptyGridText = text;
            return this;
        }

        public IGridHtmlOptions<T> SetLanguage(string lang)
        {
            _source.Language = lang;
            return this;
        }

        public IGridHtmlOptions<T> SetRowCssClasses(Func<T, string> constraint)
        {
            _source.SetRowCssClassesConstraint(constraint);
            return this;
        }

        public IGridHtmlOptions<T> Named(string gridName)
        {
            _source.RenderOptions.GridName = gridName;
            return this;
        }

        /// <summary>
        ///     Generates columns for all properties of the model.
        ///     Use data annotations to customize columns
        /// </summary>
        public IGridHtmlOptions<T> AutoGenerateColumns()
        {
            _source.AutoGenerateColumns();
            return this;
        }

        public IGridHtmlOptions<T> WithMultipleFilters()
        {
            _source.RenderOptions.AllowMultipleFilters = true;
            return this;
        }

        /// <summary>
        ///     Shows a grid items count
        /// </summary>
        /// <param name="formatString">A format string for the items count, defaults to "Items count: {0}"</param>
        public IGridHtmlOptions<T> WithGridItemsCount(string formatString = null)
        {
            if (string.IsNullOrWhiteSpace(formatString))
                formatString = "Items count: {0}";

            // For legacy compatibility
            if (!formatString.Contains("{0}"))
                formatString += ": {0}";

            _source.RenderOptions.ShowGridItemsCount = true;
            _source.RenderOptions.GridCountFormatString = formatString;
            return this;
        }

        #endregion

        private static string RenderPartialViewToString(string viewName, object model, ViewContext viewContext)
        {
            if (string.IsNullOrEmpty(viewName))
                throw new ArgumentException("viewName");

            var context = new ControllerContext(viewContext.RequestContext, viewContext.Controller);
            using (var sw = new StringWriter())
            {
                ViewEngineResult viewResult = ViewEngines.Engines.FindPartialView(context, viewName);
                if (viewResult.View == null)
                    throw new InvalidDataException(
                        string.Format("Specified view name for Grid.Mvc not found. ViewName: {0}", viewName));
                var newViewContext = new ViewContext(context, viewResult.View, viewContext.ViewData,
                                                     viewContext.TempData, sw)
                    {
                        ViewData =
                            {
                                Model = model
                            }
                    };
                viewResult.View.Render(newViewContext, sw);
                return sw.GetStringBuilder().ToString();
            }
        }
    }
}