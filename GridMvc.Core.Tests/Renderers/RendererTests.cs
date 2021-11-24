using System.Linq;
using GridMvc.Core.Columns;
using GridMvc.Core.Filtering;
using GridMvc.Core.Sorting;
using GridMvc.Core.Utility;
using Microsoft.AspNetCore.Http;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace GridMvc.Core.Tests.Renderers
{
    /// <summary>
    /// Summary description for SortTests
    /// </summary>
    [TestClass]
    public class RendererTests
    {
        private HttpContext _httpContext;

        [TestInitialize]
        public void Init()
        {
            _httpContext = Helpers.MockHttpContext();
        }

        [TestMethod]
        public void TestGridHeaderRenderer()
        {
            var renderer = new GridHeaderRenderer();
            var column = new GridColumn<TestModel, string>(c => c.Title, null);
            var htmlString = renderer.Render(column);
            Assert.IsNotNull(htmlString);
            var html = htmlString.ToHtmlString();
            Assert.IsTrue(!string.IsNullOrWhiteSpace(html));
            Assert.IsTrue(html.Contains("<th"));
            Assert.IsTrue(html.Contains("class=\"grid-header\""));
        }

        [TestMethod]
        public void TestGridCellRenderer()
        {
            var renderer = new GridCellRenderer();
            var column = new GridColumn<TestModel, string>(c => c.Title, null);
            var cell = new GridCell("test");
            var htmlString = renderer.Render(column, cell);

            Assert.IsNotNull(htmlString);
            var html = htmlString.ToHtmlString();
            Assert.IsTrue(!string.IsNullOrWhiteSpace(html));

            Assert.IsTrue(html.Contains("<td"));
            Assert.IsTrue(html.Contains(">test</td>"));
            Assert.IsTrue(html.Contains("class=\"grid-cell\""));
            Assert.IsTrue(html.Contains("data-name=\"Title\""));
        }

        [TestMethod]
        public void TestGridFilterHeaderRenderer()
        {
            var settings = new QueryStringFilterSettings(_httpContext);
            var renderer = new QueryStringFilterColumnHeaderRenderer(settings);

            var column = new GridColumn<TestModel, string>(c => c.Title, new TestGrid(Enumerable.Empty<TestModel>(), _httpContext));

            var htmlString = renderer.Render(column);
            Assert.IsNotNull(htmlString);
            var html = htmlString.ToHtmlString();
            Assert.IsTrue(string.IsNullOrEmpty(html));

            column.Filterable(true);

            htmlString = renderer.Render(column);
            Assert.IsNotNull(htmlString);
            html = htmlString.ToHtmlString();

            Assert.IsTrue(!string.IsNullOrWhiteSpace(html));

            Assert.IsTrue(html.Contains("data-filterdata="));
            Assert.IsTrue(html.Contains("class=\"grid-filter\""));
            Assert.IsTrue(html.Contains("class=\"grid-filter-btn\""));
            Assert.IsTrue(html.Contains("data-widgetdata="));
        }

        [TestMethod]
        public void TestGridSortHeaderRenderer()
        {
            var settings = new QueryStringSortSettings(_httpContext);
            var renderer = new QueryStringSortColumnHeaderRenderer(settings);

            var column = new GridColumn<TestModel, string>(c => c.Title, new TestGrid(Enumerable.Empty<TestModel>(), _httpContext));

            var htmlString = renderer.Render(column);
            Assert.IsNotNull(htmlString);
            var html = htmlString.ToHtmlString();
            Assert.IsTrue(!html.Contains("<a"));
            Assert.IsTrue(html.Contains("<span"));

            column.Sortable(true);

            htmlString = renderer.Render(column);
            Assert.IsNotNull(htmlString);
            html = htmlString.ToHtmlString();

            Assert.IsTrue(!string.IsNullOrWhiteSpace(html));
            Assert.IsTrue(html.Contains("<a"));

            Assert.IsTrue(html.Contains("class=\"grid-header-title\""));
        }
    }
}