namespace Rotativa.AspNetCore
{
    public class WkhtmltoimageDriver : WkhtmlDriver
    {
        private const string wkhtmlExe = "wkhtmltoimage.exe";
        
        /// <summary>
        /// Converts given HTML string to Image.
        /// </summary>
        /// <param name="wkhtmltoimagePath">Path to wkthmltoimage.</param>
        /// <param name="switches">Switches that will be passed to wkhtmltoimage binary.</param>
        /// <param name="html">String containing HTML code that should be converted to Image.</param>
        /// <returns>PDF as byte array.</returns>
        public static byte[] ConvertHtml(string wkhtmltoimagePath, string switches, string html)
        {
            return Convert(wkhtmltoimagePath, switches, html, wkhtmlExe);
        }
        
        /// <summary>
        /// Converts given URL to Image.
        /// </summary>
        /// <param name="wkhtmltoimagePath">Path to wkthmltoimage.</param>
        /// <param name="switches">Switches that will be passed to wkhtmltoimage binary.</param>
        /// <returns>PDF as byte array.</returns>
        public static byte[] Convert(string wkhtmltoimagePath, string switches)
        {
            return Convert(wkhtmltoimagePath, switches, null, wkhtmlExe);
        }
    }
}