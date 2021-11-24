using Microsoft.AspNetCore.Html;

namespace GridMvc.Core
{
    public interface IMasterControl
    {
        /// <summary>
        /// Render details control
        /// </summary>
        /// <param name="parent">Key element</param>
        /// <returns></returns>
        IHtmlContent RenderDetailsGrid(object parent);
    }
}
