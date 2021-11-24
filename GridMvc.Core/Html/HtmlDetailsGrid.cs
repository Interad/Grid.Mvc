using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace GridMvc.Core.Html
{
    public class HtmlDetailsGrid<TKey, TValue> : HtmlGrid<TValue>, IDetailsControl<TKey> where TKey : class
                                                                  where TValue: class
    {
        public HtmlDetailsGrid(DetailsGrid<TKey, TValue> sourceGrid, IHtmlHelper helper, ViewContext viewContext, string viewName) : base(sourceGrid, helper, viewContext, viewName)
        {
            
        }

        /*public string RenderDetailsControl(object parent)
        {
            (this._source as DetailsGrid<TKey, TValue>).RetreiveDataByKey(parent as TKey);
            return base.Render();
        }*/

        public IHtmlContent RenderDetailsControl(TKey parent)
        {
            SourceGrid.RetreiveDataByKey(parent);
            using (System.IO.StringWriter sw = new System.IO.StringWriter())
            {
                WriteTo(sw, System.Text.Encodings.Web.HtmlEncoder.Default);
                return new HtmlString(sw.ToString());
            }
        }

        protected DetailsGrid<TKey, TValue> SourceGrid
        {
            get { return (DetailsGrid<TKey, TValue>)_source; }
        }
    }
}
