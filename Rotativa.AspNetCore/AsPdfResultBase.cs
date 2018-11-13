using System;
using System.Text;
using Rotativa.AspNetCore.Options;

namespace Rotativa.AspNetCore
{
    public abstract class AsPdfResultBase : AsResultBase
    {
        protected AsPdfResultBase()
        {
            PageMargins = new Margins();
        }
        /// <summary>
        /// Sets the page size.
        /// </summary>
        [OptionFlag("-s")]
        public Size? PageSize { get; set; }

        /// <summary>
        /// Sets the page width in mm.
        /// </summary>
        /// <remarks>Has priority over <see cref="PageSize"/> but <see cref="PageHeight"/> has to be also specified.</remarks>
        [OptionFlag("--page-width")]
        public double? PageWidth { get; set; }

        /// <summary>
        /// Sets the page height in mm.
        /// </summary>
        /// <remarks>Has priority over <see cref="PageSize"/> but <see cref="PageWidth"/> has to be also specified.</remarks>
        [OptionFlag("--page-height")]
        public double? PageHeight { get; set; }

        /// <summary>
        /// Sets the page orientation.
        /// </summary>
        [OptionFlag("-O")]
        public Orientation? PageOrientation { get; set; }

        /// <summary>
        /// Sets the page margins.
        /// </summary>
        public Margins PageMargins { get; set; }

        protected override byte[] WkHtmlConvert(string switches)
        {
            return WkHtmlToPdfDriver.Convert(WkHtmlPath, switches);
        }

        protected override string GetContentType()
        {
            return "application/pdf";
        }

        /// <summary>
        /// Path to wkhtmltopdf binary.
        /// </summary>
        [Obsolete("Use WkhtmlPath instead of CookieName.", false)]
        public string WkhtmltopdfPath
        {
            get => WkHtmlPath;
            set => WkHtmlPath = value;
        }

        /// <summary>
        /// Indicates whether the PDF should be generated in lower quality.
        /// </summary>
        [OptionFlag("-l")]
        public bool IsLowQuality { get; set; }

        /// <summary>
        /// Number of copies to print into the PDF file.
        /// </summary>
        [OptionFlag("--copies")]
        public int? Copies { get; set; }

        /// <summary>
        /// Indicates whether the PDF should be generated in grayscale.
        /// </summary>
        [OptionFlag("-g")]
        public bool IsGrayScale { get; set; }

        protected override string GetConvertOptions()
        {
            var result = new StringBuilder();

            if (PageMargins != null) result.Append(PageMargins);

            result.Append(" ");
            result.Append(base.GetConvertOptions());

            return result.ToString().Trim();
        }
    }
}