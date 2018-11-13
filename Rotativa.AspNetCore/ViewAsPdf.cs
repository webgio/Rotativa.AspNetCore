using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewEngines;
using Microsoft.AspNetCore.Mvc.ViewFeatures;

namespace Rotativa.AspNetCore
{
    public class ViewAsPdf : AsPdfResultBase
    {
        private string _viewName;

        public string ViewName
        {
            get => _viewName ?? string.Empty;
            set => _viewName = value;
        }

        private string _masterName;

        public string MasterName
        {
            get => _masterName ?? string.Empty;
            set => _masterName = value;
        }

        public object Model { get; set; }

        public ViewDataDictionary ViewData { get; set; }

        public ViewAsPdf(ViewDataDictionary viewData = null)
        {
            WkHtmlPath = string.Empty;
            MasterName = string.Empty;
            ViewName = string.Empty;
            Model = null;
            ViewData = viewData;
        }

        public ViewAsPdf(string viewName, ViewDataDictionary viewData = null) : this(viewData)
        {
            ViewName = viewName;
        }

        public ViewAsPdf(object model, ViewDataDictionary viewData = null) : this(viewData)
        {
            Model = model;
        }

        public ViewAsPdf(string viewName, object model, ViewDataDictionary viewData = null) : this(viewData)
        {
            ViewName = viewName;
            Model = model;
        }

        public ViewAsPdf(string viewName, string masterName, object model) : this(viewName, model)
        {
            MasterName = masterName;
        }

        protected override string GetUrl(ActionContext context)
        {
            return string.Empty;
        }

        protected virtual ViewEngineResult GetView(ActionContext context, string viewName, string masterName)
        {
            var engine = context.HttpContext.RequestServices.GetService(typeof(ICompositeViewEngine)) as ICompositeViewEngine;
            return engine?.FindView(context, viewName, true);
        }

        protected override async Task<byte[]> CallTheDriver(ActionContext context)
        {
            string viewName = ViewName;
            if (string.IsNullOrEmpty(ViewName))
            {
                viewName = ((ControllerActionDescriptor) context.ActionDescriptor).ActionName;
            }

            var viewResult = GetView(context, viewName, MasterName);
            var tempDataProvider = context.HttpContext.RequestServices.GetService(typeof(ITempDataProvider)) as ITempDataProvider;
            var viewDataDictionary = new ViewDataDictionary(new EmptyModelMetadataProvider(), new ModelStateDictionary())
            {
                Model = Model
            };
            if (ViewData != null)
            {
                foreach (var item in ViewData)
                {
                    viewDataDictionary.Add(item);
                }
            }

            StringBuilder html;
            using (var output = new StringWriter())
            {
                var view = viewResult.View;
                var tempDataDictionary = new TempDataDictionary(context.HttpContext, tempDataProvider);
                var viewContext = new ViewContext(context, viewResult.View, viewDataDictionary, tempDataDictionary, output, new HtmlHelperOptions());

                await view.RenderAsync(viewContext);

                html = output.GetStringBuilder();
            }

            string baseUrl = string.Format("{0}://{1}", context.HttpContext.Request.Scheme, context.HttpContext.Request.Host);
            var htmlForWkHtml = Regex.Replace(html.ToString(), "<head>", $"<head><base href=\"{baseUrl}\" />", RegexOptions.IgnoreCase);

            byte[] fileContent = WkHtmlToPdfDriver.ConvertHtml(WkHtmlPath, GetConvertOptions(), htmlForWkHtml);
            return fileContent;
        }
    }
}