using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using ClosedXML.Excel;
using GridMvc.Columns;
using GridMvc.Html;

namespace GridMvc.Utility.ExportableGrid
{
    public class ExportableGrid<T> where T : class
    {
        public string Title { get; set; }
        public IEnumerable<T> Articles { get; set; }
        public ExportableGridColumnCollection<T> ColumnConfiguration { get; set; }
        public bool HasHeader { get; set; }

        // XLS
        public Action<IXLCell> FormatHeaders { get; set; }
        public Action<IXLCell> FormatValues { get; set; }
        public Action<IXLColumn> FormatColumns { get; set; }
        public Action<IXLWorksheet> FormatSheet { get; set; }

        // HTML
        public Func<IGridColumn<T>, IGridColumn<T>> FormatHtmlColumns { get; set; }
        public Action<HtmlGrid<T>> FormatHtmlGrid { get; set; }

        public ExportableGrid(string title, IEnumerable<T> articles)
        {
            Title = title;
            Articles = articles;
            HasHeader = true;
            ColumnConfiguration = new ExportableGridColumnCollection<T>();
        }

        // Fluent API:
        public ExportableGrid<T> WithHeader(bool hasHeader)
        {
            HasHeader = hasHeader;
            return this;
        }

        public ExportableGrid<T> WithColumnConfiguration(Action<ExportableGridColumnCollection<T>> columnBuilder)
        {
            columnBuilder(ColumnConfiguration);
            return this;
        }

        public ExportableGrid<T> WithFormattedHeaders(Action<IXLCell> formatHeaders)
        {
            FormatHeaders = formatHeaders;
            return this;
        }

        public ExportableGrid<T> WithFormattedValues(Action<IXLCell> formatValues)
        {
            FormatValues = formatValues;
            return this;
        }

        public ExportableGrid<T> WithFormattedColumns(Action<IXLColumn> formatColumns)
        {
            FormatColumns = formatColumns;
            return this;
        }

        public ExportableGrid<T> WithFormattedSheet(Action<IXLWorksheet> formatSheet)
        {
            FormatSheet = formatSheet;
            return this;
        }

        public ExportableGrid<T> WithFormattedHtmlColumns(Func<IGridColumn<T>, IGridColumn<T>> formatHtmlColumns)
        {
            FormatHtmlColumns = formatHtmlColumns;
            return this;
        }

        public ExportableGrid<T> WithFormattedHtmlGrid(Action<HtmlGrid<T>> formatGrid)
        {
            FormatHtmlGrid = formatGrid;
            return this;
        }

        // Methods
        public HtmlGrid<T> AsHtmlGrid(HtmlHelper htmlHelper)
        {
            var htmlGrid = (HtmlGrid<T>)htmlHelper.Grid(Articles).Columns(columns =>
            {
                foreach (var gridColumn in ColumnConfiguration)
                {
                    if (gridColumn.HideOnHtml)
                        continue;

                    var htmlColumn = columns.Add();
                    htmlColumn.RenderValueAs(x => gridColumn.Value.Compile().Invoke(x)?.ToString() ?? "");
                    htmlColumn.Titled(gridColumn.Title);
                    FormatHtmlColumns?.Invoke(htmlColumn);
                    gridColumn.FormatHtmlColumn?.Invoke(htmlColumn);
                }
            });

            FormatHtmlGrid?.Invoke(htmlGrid);
            return htmlGrid;
        }

        // XLS
        public XLWorkbook AsWorkBook()
        {
            using (var workbook = new XLWorkbook())
            {
                return AddToWorkbook(workbook);
            }
        }

        public XLWorkbook AddToWorkbook(XLWorkbook workbook)
        {
            var sheet = workbook.Worksheets.Add(Title ?? "");
            // add the headers
            if (HasHeader)
            {
                foreach (var item in ColumnConfiguration.Where(x => !x.HideOnExcel).Select((value, i) => new { Index = i + 1, Column = value })) // index starts with 1 because the column/row numbers also start w/ 1
                {
                    var cell = sheet.Cell(1, item.Index);
                    cell.Value = item.Column.Title;
                    FormatHeaders?.Invoke(cell);
                    item.Column.FormatXlsHeader?.Invoke(cell);
                }
            }

            // add the content
            var startRowIndex = HasHeader ? 2 : 1; // index starts w/ 2 because data starts in the 2nd row
            foreach (var articleItem in Articles.Select((value, i) => new { Index = i + startRowIndex, Article = value }))
            {
                foreach (var columnItem in ColumnConfiguration.Where(x => !x.HideOnExcel).Select((value, i) => new { Index = i + 1, Column = value }))
                {
                    var value = columnItem.Column.Value.Compile().Invoke(articleItem.Article);
                    var cell = sheet.Cell(articleItem.Index, columnItem.Index);
                    cell.Value = value;
                    FormatValues?.Invoke(cell);
                    columnItem.Column.FormatXlsValue?.Invoke(cell);
                }
            }

            // format the columns
            foreach (var item in ColumnConfiguration.Where(x => !x.HideOnExcel).Select((value, i) => new { Index = i + 1, Column = value })) // index starts with 1 because the column/row numbers also start w/ 1
            {
                var column = sheet.Column(item.Index);
                FormatColumns?.Invoke(column);
                item.Column.FormatXlsColumn?.Invoke(column);
            }

            FormatSheet?.Invoke(sheet);
            return workbook;
        }
    }
}
