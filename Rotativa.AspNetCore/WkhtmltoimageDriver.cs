using System.Runtime.InteropServices;

namespace Rotativa.AspNetCore
{
    public class WkhtmltoimageDriver : WkhtmlDriver
    {
        /// <summary>
        /// wkhtmltoimage only has a .exe extension in Windows.
        /// </summary>
        private static readonly string wkhtmlExe =
            RuntimeInformation.IsOSPlatform(OSPlatform.Windows) ? "wkhtmltoimage.exe" : "wkhtmltoimage";

        /// <summary>
        /// Converts given HTML string to an image.
        /// </summary>
        /// <param name="wkhtmltoimagePath">Path to wkthmltoimage.</param>
        /// <param name="switches">Switches that will be passed to wkhtmltoimage binary.</param>
        /// <param name="html">String containing HTML code that should be converted to Image.</param>
        /// <returns>Image as byte array.</returns>
        public static byte[] ConvertHtml(string wkhtmltoimagePath, string switches, string html)
        {
            return Convert(wkhtmltoimagePath, switches, html, wkhtmlExe);
        }

        /// <summary>
        /// Converts given URL to an image.
        /// </summary>
        /// <param name="wkhtmltoimagePath">Path to wkthmltoimage.</param>
        /// <param name="switches">Switches that will be passed to wkhtmltoimage binary.</param>
        /// <returns>Image as byte array.</returns>
        public static byte[] Convert(string wkhtmltoimagePath, string switches)
        {
            return Convert(wkhtmltoimagePath, switches, null, wkhtmlExe);
        }
    }
}

