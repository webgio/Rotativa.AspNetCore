using Microsoft.AspNetCore.Hosting;
using System;
using System.IO;
using System.Reflection;

namespace Rotativa.DotNet5
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
                    throw new ApplicationException("Rotativa has not been configured in Startup.cs.");
                }
                return _RotativaPath;
            }
        }
        /// <summary>
        /// Setup Rotativa library
        /// </summary>
        /// <param name="env">The IHostingEnvironment object</param>
        /// <param name="wkhtmltopdfRelativePath">Optional. Relative path to the directory containing wkhtmltopdf.exe. Default is "Rotativa". Download at https://wkhtmltopdf.org/downloads.html</param>
        public static void Setup(IWebHostEnvironment env)
        {
            var rotativaPath = string.Empty;
            if (env.EnvironmentName == "Development")
            {
                rotativaPath = Path.Combine(Path.GetDirectoryName(Assembly.GetEntryAssembly().Location), "wwwroot", "Rotativa");
            }
            else
            {
                rotativaPath = Path.Combine(env.WebRootPath, "Rotativa");
            }

            if (!Directory.Exists(rotativaPath))
            {
                throw new ApplicationException("Folder containing wkhtmltopdf.exe not found, searched for " + rotativaPath);
            }

            _RotativaPath = rotativaPath;
        }
        private static string GetAssemblyName()
        {
            var assembly = typeof(RotativaConfiguration).Assembly;
            return assembly.FullName;
        }
    }
}
