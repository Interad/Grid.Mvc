namespace GridMvc.Core
{
    /// <summary>
    ///     Renders the hiiden cells of the hidden columns
    /// </summary>
    internal class GridHiddenCellRenderer : GridCellRenderer
    {
        public GridHiddenCellRenderer()
        {
            AddCssStyle("display:none;");
        }
    }
}