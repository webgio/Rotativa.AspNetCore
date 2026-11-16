using System;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;

namespace Rotativa.AspNetCore
{
    public class ActionAsPdf : AsPdfResultBase
    {
        private RouteValueDictionary _routeValuesDict;
        private object _routeValues;
        private string _action;

        public ActionAsPdf(string action) : base()
        {
            _action = action;
        }

        public ActionAsPdf(string action, RouteValueDictionary routeValues)
            : this(action)
        {
            _routeValuesDict = routeValues;
        }

        public ActionAsPdf(string action, object routeValues)
            : this(action)
        {
            _routeValues = routeValues;
        }

        protected override string GetUrl(ActionContext  context)
        {
            var urlHelperFactory = context.HttpContext.RequestServices.GetRequiredService<IUrlHelperFactory>();
            var urlHelper = urlHelperFactory.GetUrlHelper(context);

            string actionUrl;
            if (_routeValues == null)
            {
                actionUrl = urlHelper.Action(_action, _routeValuesDict);
            }
            else if (_routeValues != null)
            {
                actionUrl = urlHelper.Action(_action, _routeValues);
            }
            else
            {
                actionUrl = urlHelper.Action(_action);
            }

            var currentUri = new Uri(context.HttpContext.Request.GetDisplayUrl());
            var authority = currentUri.GetComponents(UriComponents.StrongAuthority, UriFormat.Unescaped);

            var url = $"{context.HttpContext.Request.Scheme}://{authority}{actionUrl}";
            return url;
        }
    }
}
