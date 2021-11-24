using GridMvc.Core.Filtering;
using GridMvc.Core.Sorting;

namespace GridMvc.Core
{
    /// <summary>
    ///     Setting for grid
    /// </summary>
    public interface IGridSettingsProvider
    {
        IGridSortSettings SortSettings { get; }
        IGridFilterSettings FilterSettings { get; }
        IGridColumnHeaderRenderer GetHeaderRenderer();
    }
}