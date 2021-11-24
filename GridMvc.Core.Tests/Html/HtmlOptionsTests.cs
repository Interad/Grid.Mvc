using System.Linq;
using GridMvc.Core.Html;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace GridMvc.Core.Tests.Html
{
    [TestClass]
    public class HtmlOptionsTests
    {
        private TestGrid _grid;
        private GridHtmlOptions<TestModel> _opt;
        private HttpContext _httpContext;

        [TestInitialize]
        public void Init()
        {
            _httpContext = Helpers.MockHttpContext();

            _grid = new TestGrid(Enumerable.Empty<TestModel>(), _httpContext);
            var viewContextMock = new Mock<ViewContext>();
            var htmlHelperMock = new Mock<IHtmlHelper>();
            _opt = new GridHtmlOptions<TestModel>(_grid, htmlHelperMock.Object, viewContextMock.Object, "_Grid");
        }

        [TestMethod]
        public void TestMainMethods()
        {
            _opt.WithPaging(5);
            Assert.IsTrue(_grid.EnablePaging);
            Assert.AreEqual(_grid.Pager.PageSize, 5);

            _opt.WithMultipleFilters();
            Assert.IsTrue(_grid.RenderOptions.AllowMultipleFilters);

            _opt.Named("test");
            Assert.AreEqual(_grid.RenderOptions.GridName, "test");

            _opt.Selectable(true);
            Assert.IsTrue(_grid.RenderOptions.Selectable);

            _opt.WithGridItemsCount();
            Assert.IsTrue(_grid.RenderOptions.ShowGridItemsCount);
            Assert.AreEqual("Items count: {0}", _grid.RenderOptions.GridCountFormatString);
            Assert.AreEqual("Items count: 0", _grid.RenderGridCount());

            _opt.WithGridItemsCount("{0} in total");
            Assert.IsTrue(_grid.RenderOptions.ShowGridItemsCount);
            Assert.AreEqual("{0} in total", _grid.RenderOptions.GridCountFormatString);
            Assert.AreEqual("0 in total", _grid.RenderGridCount());
        }
    }
}
