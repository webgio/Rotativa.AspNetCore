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
        /// <summary>
        /// Setup Rotativa library
        /// </summary>
        /// <param name="env">The IHostingEnvironment object</param>
        /// <param name="wkhtmltopdfRelativePath">Optional. Relative path to the directory containing wkhtmltopdf.exe. Default is "Rotativa". Download at https://wkhtmltopdf.org/downloads.html</param>
        public static void Setup(IHostingEnvironment env, string wkhtmltopdfRelativePath = "Rotativa") 
        {
            var rotativaPath = Path.Combine(env.WebRootPath, wkhtmltopdfRelativePath);

            if (!Directory.Exists(rotativaPath))
            {
                throw new ApplicationException("Folder containing wkhtmltopdf.exe not found, searched for " + rotativaPath);
            }

            _RotativaPath = rotativaPath;
        }


        /// <summary>
        /// Setup Rotativa library
        /// </summary>
        /// <param name="RootPath">The path to the web-servable application files.</param>
        /// <param name="wkhtmltopdfRelativePath">Optional. Relative path to the directory containing wkhtmltopdf.exe. Default is "Rotativa". Download at https://wkhtmltopdf.org/downloads.html</param>
        public static void Setup(string RootPath, string wkhtmltopdfRelativePath = "Rotativa")
        {
            var rotativaPath = Path.Combine(RootPath, wkhtmltopdfRelativePath);

            if (!Directory.Exists(rotativaPath))
            {
                throw new ApplicationException("Folder containing wkhtmltopdf.exe not found, searched for " + rotativaPath);
            }

            _RotativaPath = rotativaPath;
        }

    }
}
