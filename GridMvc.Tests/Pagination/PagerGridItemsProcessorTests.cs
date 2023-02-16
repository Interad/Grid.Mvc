using GridMvc.Pagination;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

namespace GridMvc.Tests.Pagination
{
    [TestClass]
    public class PagerGridItemsProcessorTests
    {
        private PagerGridItemsProcessor<string> _processor;
        private GridPager _pager;

        [TestInitialize]
        public void Init()
        {
            HttpContext.Current = new HttpContext(new HttpRequest("", "http://tempuri.org", ""), new HttpResponse(new StringWriter()));
            _pager = new GridPager();
            _processor = new PagerGridItemsProcessor<string>(_pager);
        }

        [TestMethod]
        public void CustomPagingTest()
        {
            var list = new List<string> { "1", "2", "3", "4", "5", "6", "7", "8", "9" };

            _pager.PageSize = 5;
            _pager.ItemsCount = list.Count;
            _pager.CurrentPage = 2;

            var expectedItemsCount = 4;
            var items = _processor.Process(list.AsQueryable());
            Assert.AreEqual(expectedItemsCount, items.Count());

            _pager.CustomPaging = true;
            expectedItemsCount = 9;
            items = _processor.Process(list.AsQueryable());
            Assert.AreEqual(expectedItemsCount, items.Count());
        }
    }
}
