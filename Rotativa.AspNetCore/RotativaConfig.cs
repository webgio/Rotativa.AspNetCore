using System;
using System.IO;
using Microsoft.AspNetCore.Hosting;

namespace Rotativa.AspNetCore
{
    public static class RotativaConfiguration
    {
        private static string _rotativaPath;
        internal static string RotativaPath
        {
            get
            {
                if (string.IsNullOrEmpty(_rotativaPath))
                {
#if NET45
                    _RotativaUrl = System.Configuration.ConfigurationManager.AppSettings["RotativaUrl"];
#endif
                }
                return _rotativaPath;
            }
        }
        /// <summary>
        /// Setup Rotativa library
        /// </summary>
        /// <param name="env">The IHostingEnvironment object</param>
        /// <param name="wkHtmlToPdfRelativePath">Optional. Relative path to the directory containing wkhtmltopdf.exe. Default is "Rotativa". Download at https://wkhtmltopdf.org/downloads.html</param>
        public static void Setup(IHostingEnvironment env, string wkHtmlToPdfRelativePath = "Rotativa") 
        {
            var rotativaPath = Path.Combine(env.WebRootPath, wkHtmlToPdfRelativePath);

            if (!Directory.Exists(rotativaPath))
            {
                throw new ApplicationException("Folder containing wkhtmltopdf.exe not found, searched for " + rotativaPath);
            }

            _rotativaPath = rotativaPath;
        }
    }
}
