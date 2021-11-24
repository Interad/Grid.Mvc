using Microsoft.AspNetCore.Html;

namespace GridMvc.Core
{
    public interface IDetailsControl<TKey>
    {
        IHtmlContent RenderDetailsControl(TKey parent);
    }
}
