using System.IO;

namespace Rotativa.AspNetCore
{
    public class WkHtmlToPdfDriver : WkHtmlDriver
    {
        private readonly string _wkHtmlToPdfPath;

        public WkHtmlToPdfDriver(string wkHtmlToPdfPath)
        {
            if(File.Exists(wkHtmlToPdfPath))
            _wkHtmlToPdfPath = Path.GetFullPath(wkHtmlToPdfPath);
        }

        /// <summary>
        /// Converts given HTML string to PDF.
        /// </summary>
        /// <param name="switches">Switches that will be passed to wkhtmltopdf binary.</param>
        /// <param name="html">String containing HTML code that should be converted to PDF.</param>
        /// <returns>PDF as byte array.</returns>
        public byte[] ConvertHtml(string switches, string html)
        {
            return Convert(_wkHtmlToPdfPath, switches, html);
        }

        /// <summary>
        /// Converts given URL to PDF.
        /// </summary>
        /// <param name="switches">Switches that will be passed to wkhtmltopdf binary.</param>
        /// <returns>PDF as byte array.</returns>
        public byte[] Convert(string switches)
        {
            return Convert(_wkHtmlToPdfPath, switches, null);
        }
    }
}
