using System;
using System.Linq.Expressions;
using ClosedXML.Excel;
using GridMvc.Columns;

namespace GridMvc.Utility.ExportableGrid
{
    public class ExportableGridColumn<T> where T : class
    {
        // General Data
        public Expression<Func<T, object>> Value { get; set; }
        public string Title { get; set; }

        // XLS
        public Action<IXLCell> FormatXlsHeader { get; set; }
        public Action<IXLCell> FormatXlsValue { get; set; }
        public Action<IXLColumn> FormatXlsColumn { get; set; }
        public bool HideOnExcel { get; set; }

        // HTML
        public Func<IGridColumn<T>, IGridColumn<T>> FormatHtmlColumn { get; set; }
        public bool HideOnHtml { get; set; }

        public ExportableGridColumn()
        {
            Value = null;
        }

        public ExportableGridColumn(Expression<Func<T, object>> value)
        {
            Value = value;
            Title = GetDisplayNameFromAttribute(value);
        }

        public ExportableGridColumn<T> WithColumnHiddenOnExcel(bool hideOnExcel)
        {
            HideOnExcel = hideOnExcel;
            return this;
        }

        public ExportableGridColumn<T> WithColumnHiddenOnHtml(bool hideOnHtml)
        {
            HideOnHtml = hideOnHtml;
            return this;
        }

        public ExportableGridColumn<T> WithValueRenderedAs(Expression<Func<T, object>> value)
        {
            Value = value;
            return this;
        }

        public ExportableGridColumn<T> WithFormattedHeader(Action<IXLCell> formatHeader)
        {
            FormatXlsHeader = formatHeader;
            return this;
        }

        public ExportableGridColumn<T> WithFormattedValue(Action<IXLCell> formatValue)
        {
            FormatXlsValue = formatValue;
            return this;
        }

        public ExportableGridColumn<T> WithFormattedColumn(Action<IXLColumn> formatColumn)
        {
            FormatXlsColumn = formatColumn;
            return this;
        }

        public ExportableGridColumn<T> WithTitle(string title)
        {
            Title = title;
            return this;
        }

        public ExportableGridColumn<T> WithTitle(Expression<Func<T, object>> titleField)
        {
            Title = GetDisplayNameFromAttribute(titleField);
            return this;
        }

        public ExportableGridColumn<T> WithFormattedHtmlColumn(Func<IGridColumn<T>, IGridColumn<T>> formatHtmlColumn)
        {
            FormatHtmlColumn = formatHtmlColumn;
            return this;
        }

        private string GetDisplayNameFromAttribute(Expression<Func<T, object>> titleField)
        {
            var displayName = PropertiesHelper.GetDisplayName(titleField);
            return displayName;
        }
    }
}
