using System.Collections.Generic;
using System.Linq;
using GridMvc.Core.Pagination;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Primitives;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace GridMvc.Core.Tests.Pagination
{
    [TestClass]
    public class GridPagerTests
    {
        private GridPager _pager;
        private HttpContext _httpContext;

        [TestInitialize]
        public void Init()
        {
            _httpContext = Helpers.MockHttpContext();
            _pager = new GridPager(_httpContext);
        }

        [TestMethod]
        public void PagerPageCountTest()
        {
            _pager.ItemsCount = 1200;
            _pager.PageSize = 13;

            Assert.AreEqual(_pager.PageCount, 93);
        }

        [TestMethod]
        public void PagerDisplayingPagesTest()
        {
            _pager.ItemsCount = 1200;
            _pager.PageSize = 13;

            _pager.MaxDisplayedPages = 5;
            _pager.CurrentPage = 40;

            Assert.AreEqual(_pager.TemplateName, "_GridPager");
            Assert.AreEqual(_pager.StartDisplayedPage, 38);
            Assert.AreEqual(_pager.EndDisplayedPage, 42);
        }

        [TestMethod]
        public void PagerCurrentPageTest()
        {
            _pager.ItemsCount = 1200;
            _pager.PageSize = 13;
            _pager.CurrentPage = 1000;

            Assert.AreEqual(_pager.PageCount, _pager.CurrentPage);
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
            var queryCollection = new QueryCollection(new Dictionary<string, StringValues> { { GridPager.DefaultPageQueryParameter, "7" } });
            _httpContext = Helpers.MockHttpContext(queryCollection);
            var actual = GridPager.GetCurrentPage(_httpContext);
            Assert.AreEqual(7, actual);
        }

        [TestMethod]
        public void PagerGetCurrentPageWithCustomParameterTest()
        {
            var queryCollection = new QueryCollection(new Dictionary<string, StringValues>
            {
                { GridPager.DefaultPageQueryParameter, "9" },
                { "foo-page", new StringValues(new[] { "7", "12" }) },
                { "foo-site", "82" },
            });
            _httpContext = Helpers.MockHttpContext(queryCollection);
            var actual = GridPager.GetCurrentPage(_httpContext, "foo-page");
            Assert.AreEqual(7, actual);
        }
    }
}
