using System.ComponentModel.DataAnnotations;
using GridMvc.Core.DataAnnotations;
using Microsoft.AspNetCore.Mvc;

namespace GridMvc.Core.Tests.DataAnnotations.Models
{
    [ModelMetadataType(typeof(TestGridAnnotationMetadata))]
    internal class TestGridAnnotationModel
    {
        [GridColumn]
        public string Name { get; set; }

        public int Count { get; set; }

        [NotMappedColumn]
        public string NotMapped { get; set; }

        public string Title { get; set; }
    }

    [GridTable(PagingEnabled = true, PageSize = 20)]
    internal class TestGridAnnotationMetadata
    {
        [Display(Name = "Some title")]
        public string Title { get; set; }
    }
}
