﻿using System;
using System.Web;
using GridMvc.Columns;

namespace GridMvc.Html
{
    /// <summary>
    ///     Grid options for html helper
    /// </summary>
    public interface IGridHtmlOptions<T> : IHtmlString
    {
        IGridHtmlOptions<T> Columns(Action<IGridColumnCollection<T>> columnBuilder);

        /// <summary>
        ///     Enable paging for grid
        /// </summary>
        /// <param name="pageSize">Setup the page size of the grid</param>
        IGridHtmlOptions<T> WithPaging(int pageSize);

        /// <summary>
        ///     Enable paging for grid
        /// </summary>
        /// <param name="pageSize">Setup the page size of the grid</param>
        /// <param name="maxDisplayedItems">Setup max count of displaying pager links</param>
        IGridHtmlOptions<T> WithPaging(int pageSize, int maxDisplayedItems);

        /// <summary>
        ///     Enable paging for grid
        /// </summary>
        /// <param name="pageSize">Setup the page size of the grid</param>
        /// <param name="maxDisplayedItems">Setup max count of displaying pager links</param>
        /// <param name="queryStringParameterName">Query string parameter name</param>
        IGridHtmlOptions<T> WithPaging(int pageSize, int maxDisplayedItems, string queryStringParameterName);

        /// <summary>
        ///     Enable paging for grid
        /// </summary>
        /// <param name="pageSize">Setup the page size of the grid</param>
        /// <param name="maxDisplayedItems">Setup max count of displaying pager links</param>
        /// <param name="queryStringParameterName">Query string parameter name</param>
        /// <param name="itemsCountOverwrite">Set the total count of items</param>
        IGridHtmlOptions<T> WithPaging(int pageSize, int maxDisplayedItems, string queryStringParameterName, int? itemsCountOverwrite);

        /// <summary>
        ///     Enable sorting for all columns
        /// </summary>
        IGridHtmlOptions<T> Sortable();

        /// <summary>
        ///     Enable or disable sorting for all columns
        /// </summary>
        IGridHtmlOptions<T> Sortable(bool enable);

        /// <summary>
        ///     Enable filtering for all columns
        /// </summary>
        IGridHtmlOptions<T> Filterable();

        /// <summary>
        ///     Enable or disable filtering for all columns
        /// </summary>
        IGridHtmlOptions<T> Filterable(bool enable);

        /// <summary>
        ///     Enable or disable client grid items selectable feature
        /// </summary>
        IGridHtmlOptions<T> Selectable(bool set);

        /// <summary>
        ///     Setup the text, which will displayed with empty items collection in the grid
        /// </summary>
        /// <param name="text">Grid empty text</param>
        IGridHtmlOptions<T> EmptyText(string text);

        /// <summary>
        ///     Setup the language of Grid.Mvc
        /// </summary>
        /// <param name="lang">SetLanguage string (example: "en", "ru", "fr" etc.)</param>
        IGridHtmlOptions<T> SetLanguage(string lang);

        /// <summary>
        ///     Setup specific row css classes
        /// </summary>
        IGridHtmlOptions<T> SetRowCssClasses(Func<T, string> constraint);

        /// <summary>
        ///     Specify Grid client name
        /// </summary>
        IGridHtmlOptions<T> Named(string gridName);

        /// <summary>
        ///     Generates columns for all properties of the model.
        ///     Use data annotations to customize columns
        /// </summary>
        IGridHtmlOptions<T> AutoGenerateColumns();

        /// <summary>
        ///     Allow grid to use multiple filters
        /// </summary>
        IGridHtmlOptions<T> WithMultipleFilters();

        /// <summary>
        ///    Show grid items count
        /// </summary>
        IGridHtmlOptions<T> WithGridItemsCount(string formatString = null);

        /// <summary>
        ///     Disable default paging. Paging navigation is still displayed but the items don't get restricted to the current page and you have to take care of that yourself
        /// </summary>
        /// <param name="itemsCountOverwrite">Set the total count of items</param>
        IGridHtmlOptions<T> WithCustomPaging(int? itemsCountOverwrite);

        /// <summary>
        ///     Obviously render Grid markup
        /// </summary>
        /// <returns>Grid html layout</returns>
        string Render();
    }
}