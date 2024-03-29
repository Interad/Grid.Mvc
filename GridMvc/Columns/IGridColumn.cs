﻿using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using GridMvc.Filtering;
using GridMvc.Sorting;
using GridMvc.Utility;

namespace GridMvc.Columns
{
    public interface IGridColumn<T> : IGridColumn, IColumn<T>, ISortableColumn<T>, IFilterableColumn<T>
    {
    }

    public interface IGridColumn : ISortableColumn, IFilterableColumn
    {
        IGrid ParentGrid { get; }
    }

    /// <summary>
    ///     fluent interface for grid column
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IColumn<T>
    {
        /// <summary>
        ///     Set gridColumn title
        /// </summary>
        /// <param name="title">Title text</param>
        IGridColumn<T> Titled(string title);

        IGridColumn<T> Titled(Expression<Func<T, dynamic>> titleField);

        /// <summary>
        ///     Need to encode the content of the gridColumn
        /// </summary>
        /// <param name="encode">Yes/No</param>
        IGridColumn<T> Encoded(bool encode);

        /// <summary>
        ///     Sanitize column value from XSS attacks
        /// </summary>
        /// <param name="sanitize">If true values from this column will be sanitized</param>
        IGridColumn<T> Sanitized(bool sanitize);

        /// <summary>
        ///     Sets the width of the column
        /// </summary>
        IGridColumn<T> SetWidth(string width);

        /// <summary>
        ///     Sets the width of the column in pixels
        /// </summary>
        IGridColumn<T> SetWidth(int width);

        /// <summary>
        ///     Specify additional css class of the column
        /// </summary>
        IGridColumn<T> Css(string cssClasses);

        /// <summary>
        ///     Setup the custom renderer for property
        /// </summary>
        IGridColumn<T> RenderValueAs(Func<T, string> constraint);

        /// <summary>
        ///     Format column values with specified text pattern
        /// </summary>
        IGridColumn<T> Format(string pattern);

        /// <summary>
        ///     Supply a custom filter option that is executed
        /// </summary>
        IGridColumn<T> SetCustomFilter(Expression<Func<T, string, bool>> expression);

        IGridColumn<T> SelectListFilter(IEnumerable<GridSelectListItem> selectList);
    }

    public interface IColumn
    {
        /// <summary>
        ///     Columns title
        /// </summary>
        string Title { get; }

        /// <summary>
        ///     Internal name of the gridColumn
        /// </summary>
        string Name { get; set; }

        /// <summary>
        ///     Width of the column
        /// </summary>
        string Width { get; set; }

        /// <summary>
        ///     EncodeEnabled
        /// </summary>
        bool EncodeEnabled { get; }

        bool SanitizeEnabled { get; }

        IGridColumnHeaderRenderer HeaderRenderer { get; set; }
        IGridCellRenderer CellRenderer { get; set; }

        /// <summary>
        ///     Gets value of the gridColumn by instance
        /// </summary>
        /// <param name="instance">Instance of the item</param>
        IGridCell GetCell(object instance);
    }

    /// <summary>
    ///     fluent interface for grid sorted column
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface ISortableColumn<T> : IColumn
    {
        /// <summary>
        ///     List of column orderes
        /// </summary>
        IEnumerable<IColumnOrderer<T>> Orderers { get; }

        /// <summary>
        ///     Enable sort of the gridColumn
        /// </summary>
        /// <param name="sort">Yes/No</param>
        IGridColumn<T> Sortable(bool sort);

        /// <summary>
        ///     Setup the initial sorting direction of current column
        /// </summary>
        /// <param name="direction">Ascending / Descending</param>
        IGridColumn<T> SortInitialDirection(GridSortDirection direction);

        /// <summary>
        ///     Setup ThenBy sorting of current column
        /// </summary>
        IGridColumn<T> ThenSortBy<TKey>(Expression<Func<T, TKey>> expression);

        /// <summary>
        ///     Setup ThenByDescending sorting of current column
        /// </summary>
        IGridColumn<T> ThenSortByDescending<TKey>(Expression<Func<T, TKey>> expression);
    }

    public interface ISortableColumn : IColumn
    {
        /// <summary>
        ///     Enable sort for this column
        /// </summary>
        bool SortEnabled { get; }

        /// <summary>
        ///     Is current column sorted
        /// </summary>
        bool IsSorted { get; set; }

        /// <summary>
        ///     Sort direction of current column
        /// </summary>
        GridSortDirection? Direction { get; set; }
    }

    public interface IFilterableColumn<T>
    {
        /// <summary>
        ///     Collection of current column filter
        /// </summary>
        IColumnFilter<T> Filter { get; }

        /// <summary>
        ///     Allows filtering for this column
        /// </summary>
        /// <param name="enable">Enable/disable filtering</param>
        IGridColumn<T> Filterable(bool enable);

        /// <summary>
        ///     Set up initial filter for this column
        /// </summary>
        /// <param name="type">Filter type</param>
        /// <param name="value">Filter value</param>
        IGridColumn<T> SetInitialFilter(GridFilterType type, string value);

        /// <summary>
        ///     Specify custom filter widget type for this column
        /// </summary>
        /// <param name="typeName">Widget type name</param>
        IGridColumn<T> SetFilterWidgetType(string typeName);

        /// <summary>
        ///     Specify custom filter widget type for this column
        /// </summary>
        /// <param name="typeName">Widget type name</param>
        /// <param name="widgetData">The data would be passed to the widget</param>
        IGridColumn<T> SetFilterWidgetType(string typeName, object widgetData);
    }

    public interface IFilterableColumn : IColumn
    {
        /// <summary>
        ///     Internal name of the gridColumn
        /// </summary>
        bool FilterEnabled { get; }

        /// <summary>
        ///     Initial filter settings for the column
        /// </summary>
        ColumnFilterValue InitialFilterSettings { get; set; }

        string FilterWidgetTypeName { get; }

        object FilterWidgetData { get; }
    }
}