namespace Rotativa.AspNetCore
{
    public class WkHtmlToPdfDriver : WkHtmlDriver
    {
        private const string WkHtmlExe = "wkhtmltopdf.exe";

        /// <summary>
        /// Converts given HTML string to PDF.
        /// </summary>
        /// <param name="wkHtmlToPdfPath">Path to wkthmltopdf.</param>
        /// <param name="switches">Switches that will be passed to wkhtmltopdf binary.</param>
        /// <param name="html">String containing HTML code that should be converted to PDF.</param>
        /// <returns>PDF as byte array.</returns>
        public static byte[] ConvertHtml(string wkHtmlToPdfPath, string switches, string html)
        {
            return Convert(wkHtmlToPdfPath, switches, html, WkHtmlExe);
        }

        /// <summary>
        /// Converts given URL to PDF.
        /// </summary>
        /// <param name="wkHtmlToPdfPath">Path to wkthmltopdf.</param>
        /// <param name="switches">Switches that will be passed to wkhtmltopdf binary.</param>
        /// <returns>PDF as byte array.</returns>
        public static byte[] Convert(string wkHtmlToPdfPath, string switches)
        {
            return Convert(wkHtmlToPdfPath, switches, null, WkHtmlExe);
        }
    }
}
