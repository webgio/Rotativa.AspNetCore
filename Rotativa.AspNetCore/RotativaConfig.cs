using Microsoft.AspNetCore.Hosting;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Rotativa.AspNetCore
{
    public static class RotativaConfiguration
    {
        private static string _RotativaPath;
        internal static string RotativaPath
        {
            get
            {
                if (string.IsNullOrEmpty(_RotativaPath))
                {
#if NET45
                    _RotativaUrl = System.Configuration.ConfigurationManager.AppSettings["RotativaUrl"];
#endif
                }
                return _RotativaPath;
            }
        }
        public static void Setup(IHostingEnvironment env, string wkhtmltopdfRelativePath = "Rotativa") 
        {
            var rotativaPath = Path.Combine(env.WebRootPath, wkhtmltopdfRelativePath);

            _RotativaPath = rotativaPath;
        }

    }
}
