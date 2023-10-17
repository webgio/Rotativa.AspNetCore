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

#if NETSTANDARD2_0_OR_GREATER
        /// <summary>
        /// Setup Rotativa library
        /// </summary>
        /// <param name="env">The IHostingEnvironment object</param>
        /// <param name="wkhtmltopdfRelativePath">Optional. Relative path to the directory containing wkhtmltopdf.exe. Default is "Rotativa". Download at https://wkhtmltopdf.org/downloads.html</param>
        public static void Setup(Microsoft.AspNetCore.Hosting.IHostingEnvironment env, string wkhtmltopdfRelativePath = "Rotativa") 
        {
            var rotativaPath = Path.Combine(env.WebRootPath, wkhtmltopdfRelativePath);

            if (!Directory.Exists(rotativaPath))
            {
                throw new ApplicationException("Folder containing wkhtmltopdf.exe not found, searched for " + rotativaPath);
            }

            _RotativaPath = rotativaPath;
        }
#endif

        /// <summary>
        /// Setup Rotativa library
        /// </summary>
        /// <param name="rootPath">The path to the web-servable application files.</param>
        /// <param name="wkhtmltopdfRelativePath">Optional. Relative path to the directory containing wkhtmltopdf.exe. Default is "Rotativa". Download at https://wkhtmltopdf.org/downloads.html</param>
        public static void Setup(string rootPath, string wkhtmltopdfRelativePath = "Rotativa")
        {
            var rotativaPath = Path.Combine(rootPath, wkhtmltopdfRelativePath);

            if (!Directory.Exists(rotativaPath))
            {
                throw new ApplicationException("Folder containing wkhtmltopdf.exe not found, searched for " + rotativaPath);
            }

            _RotativaPath = rotativaPath;
        }

    }
}
