using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using Rotativa.AspNetCore.Options;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewEngines;

using Microsoft.AspNetCore.Mvc.ViewFeatures;

#if NET5_0_OR_GREATER
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Http;
#elif NETCOREAPP3_1
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;
#elif NETSTANDARD2_0
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;
#endif

namespace Rotativa.AspNetCore
{
    public abstract class AsResultBase : ViewResult
    {
        protected AsResultBase()
        {
            this.WkhtmlPath = string.Empty;
            this.FormsAuthenticationCookieName = ".ASPXAUTH";
            this.IsPartialView = false;
            this.SetBaseUrl = true;
        }

        /// <summary>
        /// Determines if the view that is referenced is partial or not.
        /// </summary>
        public bool IsPartialView { get; set; }

        /// <summary>
        /// Whether we add a base URL when we generate the HTML.
        /// </summary>
        /// <remarks>
        /// This was always on because it wasn't configurable (<= 1.3.2). That's why the default is on.
        /// However it's cleaner to allow developers to set it themselves, and only add the BaseUrl when requested.
        /// </remarks>
        public bool SetBaseUrl { get; set; }

        /// <summary>
        /// This will be send to the browser as a name of the generated PDF file.
        /// </summary>
        public string FileName { get; set; }

        /// <summary>
        /// Path to wkhtmltopdf / wkhtmltoimage binary.
        /// </summary>
        public string WkhtmlPath { get; set; }

        /// <summary>
        /// Custom name of authentication cookie used by forms authentication.
        /// </summary>
        [Obsolete("Use FormsAuthenticationCookieName instead of CookieName.")]
        public string CookieName
        {
            get { return this.FormsAuthenticationCookieName; }
            set { this.FormsAuthenticationCookieName = value; }
        }

        /// <summary>
        /// Custom name of authentication cookie used by forms authentication.
        /// </summary>
        public string FormsAuthenticationCookieName { get; set; }

        /// <summary>
        /// Sets custom headers.
        /// </summary>
        [OptionFlag("--custom-header")]
        public Dictionary<string, string> CustomHeaders { get; set; }

        /// <summary>
        /// Sets cookies.
        /// </summary>
        [OptionFlag("--cookie")]
        public Dictionary<string, string> Cookies { get; set; }

        /// <summary>
        /// Sets post values.
        /// </summary>
        [OptionFlag("--post")]
        public Dictionary<string, string> Post { get; set; }

        /// <summary>
        /// Indicates whether the page can run JavaScript.
        /// </summary>
        [OptionFlag("-n")]
        public bool IsJavaScriptDisabled { get; set; }

        /// <summary>
        /// Minimum font size.
        /// </summary>
        [OptionFlag("--minimum-font-size")]
        public int? MinimumFontSize { get; set; }

        /// <summary>
        /// Sets proxy server.
        /// </summary>
        [OptionFlag("-p")]
        public string Proxy { get; set; }

        /// <summary>
        /// HTTP Authentication username.
        /// </summary>
        [OptionFlag("--username")]
        public string UserName { get; set; }

        /// <summary>
        /// HTTP Authentication password.
        /// </summary>
        [OptionFlag("--password")]
        public string Password { get; set; }

        /// <summary>
        /// Use this if you need another switches that are not currently supported by Rotativa.
        /// </summary>
        [OptionFlag("")]
        public string CustomSwitches { get; set; }

        [Obsolete(@"Use BuildFile(this.ControllerContext) method instead and use the resulting binary data to do what needed.")]
        public string SaveOnServerPath { get; set; }

        public ContentDisposition ContentDisposition { get; set; }

        protected abstract string GetUrl(Microsoft.AspNetCore.Mvc.ActionContext context);

        /// <summary>
        /// Returns properties with OptionFlag attribute as one line that can be passed to wkhtmltopdf / wkhtmltoimage binary.
        /// </summary>
        /// <returns>Command line parameter that can be directly passed to wkhtmltopdf / wkhtmltoimage binary.</returns>
        protected virtual string GetConvertOptions()
        {
            var result = new StringBuilder();

            var fields = this.GetType().GetProperties();
            foreach (var fi in fields)
            {
                var of = fi.GetCustomAttributes(typeof(OptionFlag), true).FirstOrDefault() as OptionFlag;
                if (of == null)
                    continue;

                object value = fi.GetValue(this, null);
                if (value == null)
                    continue;

                if (fi.PropertyType == typeof(Dictionary<string, string>))
                {
                    var dictionary = (Dictionary<string, string>)value;
                    foreach (var d in dictionary)
                    {
                        result.AppendFormat(" {0} {1} {2}", of.Name, d.Key, d.Value);
                    }
                }
                else if (fi.PropertyType == typeof(bool))
                {
                    if ((bool)value)
                        result.AppendFormat(CultureInfo.InvariantCulture, " {0}", of.Name);
                }
                else
                {
                    result.AppendFormat(CultureInfo.InvariantCulture, " {0} {1}", of.Name, value);
                }
            }

            return result.ToString().Trim();
        }

        private string GetWkParams(ActionContext context)
        {
            var switches = string.Empty;

            string authenticationCookie = null;
            if (context.HttpContext.Request.Cookies != null && context.HttpContext.Request.Cookies.Keys.Contains(FormsAuthenticationCookieName))
            {
                authenticationCookie = context.HttpContext.Request.Cookies[FormsAuthenticationCookieName];
            }
            if (authenticationCookie != null)
            {
                //var authCookieValue = authenticationCookie.Value;
                switches += " --cookie " + this.FormsAuthenticationCookieName + " " + authenticationCookie;
            }

            switches += " " + this.GetConvertOptions();

            var url = this.GetUrl(context);
            switches += " " + url;

            return switches;
        }

        protected virtual async Task<byte[]> CallTheDriver(ActionContext context)
        {
            var switches = this.GetWkParams(context);
            var fileContent = this.WkhtmlConvert(switches);
            return fileContent;
        }
        //protected abstract Task<byte[]> CallTheDriver(ActionContext context);

        protected abstract byte[] WkhtmlConvert(string switches);

        public async Task<byte[]> BuildFile(ActionContext context)
        {
            if (context == null)
                throw new ArgumentNullException("context");

            //if (this.WkhtmlPath == string.Empty)
            //    this.WkhtmlPath = context.HttpContext.Server.MapPath("~/Rotativa");

            this.WkhtmlPath = RotativaConfiguration.RotativaPath;

            var fileContent = await CallTheDriver(context);

            if (string.IsNullOrEmpty(this.SaveOnServerPath) == false)
            {
                File.WriteAllBytes(this.SaveOnServerPath, fileContent);
            }

            return fileContent;
        }

        public async override Task ExecuteResultAsync(ActionContext context)
        {
            var fileContent = await this.BuildFile(context);

            var response = this.PrepareResponse(context.HttpContext.Response);

            await response.Body.WriteAsync(fileContent, 0, fileContent.Length);
        }

        private static string SanitizeFileName(string name)
        {
            string invalidChars = Regex.Escape(new string(Path.GetInvalidPathChars()) + new string(Path.GetInvalidFileNameChars()));
            string invalidCharsPattern = string.Format(@"[{0}]+", invalidChars);

            string result = Regex.Replace(name, invalidCharsPattern, "_");
            return result;
        }

        protected HttpResponse PrepareResponse(HttpResponse response)
        {
            response.ContentType = this.GetContentType();

            if (!String.IsNullOrEmpty(this.FileName))
            {
                var contentDisposition = this.ContentDisposition == ContentDisposition.Attachment
                    ? "attachment"
                    : "inline";

                response.Headers.Add("Content-Disposition", string.Format("{0}; filename=\"{1}\"", contentDisposition, SanitizeFileName(this.FileName)));
            }
            //response.Headers.Add("Content-Type", this.GetContentType());

            return response;
        }

        protected abstract string GetContentType();

        /// <summary>
        /// Get the view out of the context.
        /// </summary>
        /// <param name="context">The action context</param>
        /// <returns></returns>
        /// <exception cref="InvalidOperationException"></exception>
        protected IView GetView(ActionContext context)
        {
            // Use current action name if the view name was not provided
            if (string.IsNullOrEmpty(ViewName))
            {
                ViewName = ((Microsoft.AspNetCore.Mvc.Controllers.ControllerActionDescriptor)context.ActionDescriptor).ActionName;
            }

            var engine = context.HttpContext.RequestServices.GetService(typeof(ICompositeViewEngine)) as ICompositeViewEngine;
            var getViewResult = engine.GetView(executingFilePath: null, viewPath: ViewName, isMainPage: !IsPartialView);
            if (getViewResult.Success)
            {
                return getViewResult.View;
            }

            var findViewResult = engine.FindView(context, ViewName, isMainPage: !IsPartialView);
            if (findViewResult.Success)
            {
                return findViewResult.View;
            }

            var searchedLocations = getViewResult.SearchedLocations.Concat(findViewResult.SearchedLocations);
            var errorMessage = string.Join(
                System.Environment.NewLine,
                new[] { $"Unable to find view '{ViewName}'. The following locations were searched:" }.Concat(searchedLocations));

            throw new InvalidOperationException(errorMessage);
        }

        protected async Task<string> GetHtmlFromView(ActionContext context)
        {
            var view = GetView(context);
            var html = new StringBuilder();

            ITempDataProvider tempDataProvider = context.HttpContext.RequestServices.GetService(typeof(ITempDataProvider)) as ITempDataProvider;
            var tempDataDictionary = new TempDataDictionary(context.HttpContext, tempDataProvider);

            using (var output = new StringWriter())
            {
                var viewContext = new ViewContext(
                    context,
                    view,
                    this.ViewData,
                    tempDataDictionary,
                    output,
                    new HtmlHelperOptions());

                await view.RenderAsync(viewContext);

                html = output.GetStringBuilder();
            }

            if (!this.SetBaseUrl)
            {
                return html.ToString();
            }

            string baseUrl = string.Format("{0}://{1}", context.HttpContext.Request.Scheme, context.HttpContext.Request.Host);
            return Regex.Replace(html.ToString(), "<head>", string.Format("<head><base href=\"{0}\" />", baseUrl), RegexOptions.IgnoreCase);
        }
    }
}