using GridMvc.Core.DataAnnotations;
using GridMvc.Core.Tests.DataAnnotations.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace GridMvc.Core.Tests.DataAnnotations
{
    [TestClass]
    public class DataAnnotationsProviderTests
    {
        private IGridAnnotationsProvider _provider;
        [TestInitialize]
        public void Init()
        {
            _provider = new GridAnnotationsProvider();
        }

        [TestMethod]
        public void TestProviderMetadataType()
        {
            var pi = typeof(TestGridAnnotationModel).GetProperty("Title");
            var opt = _provider.GetAnnotationForColumn<TestGridAnnotationModel>(pi);
            Assert.IsNotNull(opt);
            Assert.AreEqual("Some title", opt.Title);

            var gridSettings = _provider.GetAnnotationForTable<TestGridAnnotationModel>();
            Assert.IsNotNull(gridSettings);
            Assert.AreEqual(gridSettings.PageSize, 20);
            Assert.AreEqual(gridSettings.PagingEnabled, true);
        }
    }
}
