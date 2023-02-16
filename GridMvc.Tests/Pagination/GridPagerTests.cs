using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using GridMvc.Pagination;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace GridMvc.Tests.Pagination
{
    [TestClass]
    public class GridPagerTests
    {
        private GridPager _pager;

        [TestInitialize]
        public void Init()
        {
            HttpContext.Current = new HttpContext(new HttpRequest("", "http://tempuri.org", ""), new HttpResponse(new StringWriter()));
            _pager = new GridPager();
        }

        [TestMethod]
        public void PagerPageCountTest()
        {
            _pager.ItemsCount = 1200;
            _pager.PageSize = 13;

            Assert.AreEqual(93, _pager.PageCount);
        }

        [TestMethod]
        public void PagerDisplayingPagesTest()
        {
            _pager.ItemsCount = 1200;
            _pager.PageSize = 13;

            _pager.MaxDisplayedPages = 5;
            _pager.CurrentPage = 40;

            Assert.AreEqual("_GridPager", _pager.TemplateName);
            Assert.AreEqual(38, _pager.StartDisplayedPage);
            Assert.AreEqual(42, _pager.EndDisplayedPage);
        }

        [TestMethod]
        public void PagerCurrentPageTest()
        {
            _pager.ItemsCount = 1200;
            _pager.PageSize = 13;
            _pager.CurrentPage = 1000;

            Assert.AreEqual(_pager.CurrentPage, _pager.PageCount);
        }

        [TestMethod]
        public void PagerItemsCountTest()
        {
            var list = new List<string> { "1", "2" };
            _pager.Initialize(list.AsQueryable());

            Assert.AreEqual(list.Count, _pager.ItemsCount);
        }

        [TestMethod]
        public void PagerItemsCountOverwriteTest()
        {
            const int expectedItemsCount = 2768;
            var list = new List<string> { "1", "2" };
            _pager.ItemsCountOverwrite = expectedItemsCount;
            _pager.Initialize(list.AsQueryable());

            Assert.AreEqual(expectedItemsCount, _pager.ItemsCount);
        }

        [TestMethod]
        public void PagerGetCurrentPageTest()
        {
            var context = new HttpContext(new HttpRequest("", "http://tempuri.org", $"{GridPager.DefaultPageQueryParameter}=7"), new HttpResponse(new StringWriter()));
            var actual = GridPager.GetCurrentPage(context);
            Assert.AreEqual(7, actual);
        }

        [TestMethod]
        public void PagerGetCurrentPageWithCustomParameterTest()
        {
            var context = new HttpContext(new HttpRequest("", "http://tempuri.org", $"{GridPager.DefaultPageQueryParameter}=9&foo-page=7&foo-site=82"), new HttpResponse(new StringWriter()));
            var actual = GridPager.GetCurrentPage(context, "foo-page");
            Assert.AreEqual(7, actual);
        }

        [TestMethod]
        public void PagerGetSkipForCurrentPageTest()
        {
            var pageSize = 100;
            var currentPage = 3;
            var skip = GridPager.GetSkip(pageSize, currentPage);
            Assert.AreEqual(200, skip);
        }

        [TestMethod]
        public void PagerGetSkipForCurrentPageUriTest()
        {
            var pageSize = 100;
            var context = new HttpContext(new HttpRequest("", "http://tempuri.org", $"{GridPager.DefaultPageQueryParameter}=3"), new HttpResponse(new StringWriter()));
            var skip = GridPager.GetSkip(pageSize, context);
            Assert.AreEqual(200, skip);
        }

        [TestMethod]
        public void PagerGetSkipForCurrentPageUriWithCustomParameterTest()
        {
            var pageSize = 100;
            var context = new HttpContext(new HttpRequest("", "http://tempuri.org", $"{GridPager.DefaultPageQueryParameter}=3&foo-page=7&foo-site=82"), new HttpResponse(new StringWriter()));
            var skip = GridPager.GetSkip(pageSize, context, "foo-page");
            Assert.AreEqual(600, skip);
        }
    }
}
