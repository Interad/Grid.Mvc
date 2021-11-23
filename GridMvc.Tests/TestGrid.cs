using System.Collections.Generic;

namespace GridMvc.Tests
{
    public class TestGrid : Grid<TestModel>
    {
        public TestGrid(IEnumerable<TestModel> items)
            : base(items)
        {
        }
    }
}