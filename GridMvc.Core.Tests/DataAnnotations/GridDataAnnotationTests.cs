using System.Linq;
using System.Reflection;
using GridMvc.Core.Columns;
using GridMvc.Core.DataAnnotations;
using GridMvc.Core.Tests.DataAnnotations.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace GridMvc.Core.Tests.DataAnnotations
{
    [TestClass]
    public class GridDataAnnotationTests
    {
        private Grid<TestGridAnnotationModel> _grid;
        private HttpContext _httpContext;

        [TestInitialize]
        public void Init()
        {
            _httpContext = Helpers.MockHttpContext();
            _grid = new Grid<TestGridAnnotationModel>(Enumerable.Empty<TestGridAnnotationModel>().AsQueryable(), _httpContext);
        }

        [TestMethod]
        public void TestPaging()
        {
            Assert.AreEqual(true, _grid.EnablePaging);
            Assert.AreEqual(20, _grid.Pager.PageSize);
        }

        [TestMethod]
        public void TestColumnsDataAnnotation()
        {
            _grid.AutoGenerateColumns();
            int i = 0;
            foreach (var pi in typeof(TestGridAnnotationModel).GetProperties(BindingFlags.Public | BindingFlags.Instance))
            {
                if (pi.GetAttribute<NotMappedColumnAttribute>() != null)
                    continue;

                var gridOpt = pi.GetAttribute<GridColumnAttribute>();

                if (gridOpt != null)
                {
                    var column = _grid.Columns.ElementAt(i) as IGridColumn<TestGridAnnotationModel>;
                    if (column == null)
                        Assert.Fail();

                    Assert.AreEqual(gridOpt.EncodeEnabled, column.EncodeEnabled);
                    Assert.AreEqual(gridOpt.FilterEnabled, column.FilterEnabled);
                    Assert.AreEqual(gridOpt.SanitizeEnabled, column.SanitizeEnabled);

                    if (!string.IsNullOrEmpty(gridOpt.Title))
                        Assert.AreEqual(gridOpt.Title, column.Title);

                    if (!string.IsNullOrEmpty(gridOpt.Width))
                        Assert.AreEqual(gridOpt.Width, column.Width);
                }
                i++;
            }
            Assert.AreEqual(3, _grid.Columns.Count());
        }
    }
}
