using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace Rotativa.AspNetCore
{
#if NET6_0_OR_GREATER
    public static class MvcServiceCollectionExtensions
    {
        public static IApplicationBuilder UseRotativa(this IApplicationBuilder app) 
        {
            var webApp = app as WebApplication;

            if (webApp == null) 
            {
                throw new Exception("Sorry, you can use Rotativa only in a WebApplication");
            }

            RotativaConfiguration.Setup(webApp.Environment.WebRootPath);

            return app;

        }

    }
#endif
}
