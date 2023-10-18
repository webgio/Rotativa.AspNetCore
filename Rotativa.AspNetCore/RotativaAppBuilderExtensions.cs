
using System;
#if NETCOREAPP3_1
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Builder;
#elif NET5_0
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Builder;
#elif NET6_0_OR_GREATER
using Microsoft.AspNetCore.Builder;
#endif


namespace Rotativa.AspNetCore
{
#if NET5_0
    public static class RotativaAppBuilderExtensions
    {
        public static IApplicationBuilder UseRotativa(this IApplicationBuilder app, Microsoft.AspNetCore.Hosting.IWebHostEnvironment env) 
        {
            var webApp = app as ApplicationBuilder;

            if (webApp == null) 
            {
                throw new Exception("Sorry, you can use Rotativa only in a WebApplication");
            }

            RotativaConfiguration.Setup(env.WebRootPath);

            return app;

        }

    }
#endif

#if NETCOREAPP3_1
    public static class RotativaAppBuilderExtensions
    {
        public static IApplicationBuilder UseRotativa(this IApplicationBuilder app, Microsoft.AspNetCore.Hosting.IWebHostEnvironment env) 
        {
            var webApp = app as ApplicationBuilder;

            if (webApp == null) 
            {
                throw new Exception("Sorry, you can use Rotativa only in a WebApplication");
            }

            RotativaConfiguration.Setup(env.WebRootPath);

            return app;

        }

    }
#endif

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
# endif

}
