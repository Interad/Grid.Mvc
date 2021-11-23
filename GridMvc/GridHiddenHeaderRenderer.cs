namespace GridMvc
{
    /// <summary>
    ///     Renders the hidden cells of the hidden columns
    /// </summary>
    internal class GridHiddenHeaderRenderer : GridHeaderRenderer
    {
        public GridHiddenHeaderRenderer()
        {
            AddCssStyle("display:none;");
        }
    }
}