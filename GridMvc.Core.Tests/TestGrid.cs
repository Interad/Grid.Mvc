using System.Collections.Generic;
using Microsoft.AspNetCore.Http;

namespace GridMvc.Core.Tests
{
    public class TestGrid : Grid<TestModel>
    {
        public TestGrid(IEnumerable<TestModel> items, HttpContext httpContext)
            : base(items, httpContext)
        {
        }
    }
}