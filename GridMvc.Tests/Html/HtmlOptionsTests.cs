using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using GridMvc.Html;
using GridMvc.Pagination;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace GridMvc.Tests.Html
{
    [TestClass]
    public class HtmlOptionsTests
    {
        private TestGrid _grid;
        private GridHtmlOptions<TestModel> _opt;

        [TestInitialize]
        public void Init()
        {
            HttpContext.Current = new HttpContext(new HttpRequest("", "http://tempuri.org", ""), new HttpResponse(new StringWriter()));
            _grid = new TestGrid(Enumerable.Empty<TestModel>());
            var viewContextMock = new Mock<ViewContext>();
            _opt = new GridHtmlOptions<TestModel>(_grid, viewContextMock.Object, "_Grid");
        }

        [TestMethod]
        public void WithPagingTest()
        {
            Assert.IsFalse(_grid.EnablePaging, "Default is no paging");

            _opt.WithPaging(5);
            Assert.IsTrue(_grid.EnablePaging, "Paging should be enabled after WithPaging");
            Assert.AreEqual(5, _grid.Pager.PageSize, "PageSize should be the parameter");

            _opt.WithPaging(20);
            Assert.AreEqual(20, _grid.Pager.PageSize, "PageSize should be the latest value");

            _opt.WithPaging(20, 10);
            Assert.AreEqual(10, (_grid.Pager as GridPager)?.MaxDisplayedPages, "MaxDisplayedPages should be updated");

            _opt.WithPaging(20, 10, null);
            Assert.AreEqual(GridPager.DefaultPageQueryParameter, (_grid.Pager as GridPager)?.ParameterName, "ParameterName should be default if none given");

            _opt.WithPaging(20, 10, "page");
            Assert.AreEqual("page", (_grid.Pager as GridPager)?.ParameterName, "ParameterName should be updated");

            _opt.WithPaging(20, 10, null);
            Assert.AreEqual("page", (_grid.Pager as GridPager)?.ParameterName, "ParameterName should be kept if no new given");

            _opt.WithPaging(7, 10, null, 2768);
            Assert.AreEqual(2768, _grid.Pager.ItemsCountOverwrite, "ItemsCountOverwrite should have the manually set value");
        }

        [TestMethod]
        public void WithMultipleFiltersTest()
        {
            Assert.IsFalse(_grid.RenderOptions.AllowMultipleFilters);

            _opt.WithMultipleFilters();
            Assert.IsTrue(_grid.RenderOptions.AllowMultipleFilters);
        }

        [TestMethod]
        public void NamedTest()
        {
            _opt.Named("test");
            Assert.AreEqual(_grid.RenderOptions.GridName, "test");
        }

        [TestMethod]
        public void SelectableTest()
        {
            Assert.IsTrue(_grid.RenderOptions.Selectable, "Default should be true");

            _opt.Selectable(false);
            Assert.IsFalse(_grid.RenderOptions.Selectable, "Selectable should be disabled now");

            _opt.Selectable(true);
            Assert.IsTrue(_grid.RenderOptions.Selectable, "Selectable should be enabled again");
        }

        [TestMethod]
        public void WithGridItemsCountTest()
        {
            Assert.IsFalse(_grid.RenderOptions.ShowGridItemsCount);

            _opt.WithGridItemsCount();
            Assert.IsTrue(_grid.RenderOptions.ShowGridItemsCount);
            Assert.AreEqual("Items count: {0}", _grid.RenderOptions.GridCountFormatString);
            Assert.AreEqual("Items count: 0", _grid.RenderGridCount());
        }

        [TestMethod]
        public void WithGridItemsCountCustomTest()
        {
            _opt.WithGridItemsCount("{0} in total");
            Assert.IsTrue(_grid.RenderOptions.ShowGridItemsCount);
            Assert.AreEqual("{0} in total", _grid.RenderOptions.GridCountFormatString);
            Assert.AreEqual("0 in total", _grid.RenderGridCount());
        }
    }
}
