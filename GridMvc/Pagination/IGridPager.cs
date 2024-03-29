﻿using System.Linq;

namespace GridMvc.Pagination
{
    public interface IGridPager
    {
        /// <summary>
        ///     Max grid items, displaying on the page
        /// </summary>
        int PageSize { get; set; }

        /// <summary>
        ///     Current page index
        /// </summary>
        int CurrentPage { get; }

        /// <summary>
        ///     Partial view name to render the pager
        /// </summary>
        string TemplateName { get; }

        /// <summary>
        ///     Total count of items, overrules default ItemsCount
        /// </summary>
        int? ItemsCountOverwrite { get; set; }

        /// <summary>
        ///     Disables default paging if set to true
        ///     Paging navigation is still displayed but the items don't get restricted to the current page and you have to take care of that yourself
        /// </summary>
        bool CustomPaging { get; set; }

        /// <summary>
        ///     Method invokes before pager render
        /// </summary>
        void Initialize<T>(IQueryable<T> items);

        ///// <summary>
        /////     Total pages count
        ///// </summary>
        //int PageCount { get; }

        ///// <summary>
        /////     Starting displaying page
        ///// </summary>
        //int StartDisplayedPage { get; }

        ///// <summary>
        /////     Last displaying page
        ///// </summary>
        //int EndDisplayedPage { get; }

        //int MaxDisplayedPages { get; set; }

        //string ParameterName { get; }

        //int ItemsCount { get; set; }

        ///// <summary>
        /////     Get the address for a specific page
        ///// </summary>
        ///// <param name="pageIndex">Page number</param>
        ///// <returns>Page address</returns>
        //string GetLinkForPage(int pageIndex);
    }
}