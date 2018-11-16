using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Rotativa.AspNetCore.Options
{
    public class Options
    {
        /// <summary>
        /// Set the page bottom margin
        /// </summary>
        [OptionFlag("-B")] 
        public int? Bottom { get; set; }

        /// <summary>
        /// Set the page left margin (default 10mm)
        /// </summary>
        [OptionFlag("-L")] 
        public int? Left { get; set; }

        /// <summary>
        /// Set the page right margin (default 10mm)
        /// </summary>
        [OptionFlag("-R")] 
        public int? Right { get; set; }

        /// <summary>
        /// Set the page top margin
        /// </summary>
        [OptionFlag("-T")] 
        public int? Top { get; set; }

        /// <summary>
        /// Set paper size to: A4, Letter, etc.
        /// (default A4)
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
        /// Set orientation to Landscape or Portrait
        /// (default Portrait)
        /// </summary>
        [OptionFlag("-O")]
        public Orientation? PageOrientation { get; set; }

        /// <summary>
        /// Generates lower quality pdf/ps.
        /// Useful to shrink the result document space
        /// </summary>
        [OptionFlag("-l")]
        public bool? IsLowQuality { get; set; }

        /// <summary>
        /// Number of copies to print into the pdf file
        /// (default 1)
        /// </summary>
        [OptionFlag("--copies")]
        public int? Copies { get; set; }

        /// <summary>
        /// PDF will be generated in grayscale
        /// </summary>
        [OptionFlag("-g")]
        public bool? IsGrayScale { get; set; }
        
        /// <summary>
        /// Set an additional HTTP header (repeatable)
        /// </summary>
        [OptionFlag("--custom-header")]
        public Dictionary<string, string> CustomHeaders { get; set; }
        
        /// <summary>
        /// Set an additional cookie (repeatable),
        /// value should be url encoded.
        /// </summary>
        [OptionFlag("--cookie")]
        public Dictionary<string, string> Cookies { get; set; }
        
        /// <summary>
        /// Add an additional post field (repeatable).
        /// </summary>
        [OptionFlag("--post")]
        public Dictionary<string, string> Post { get; set; }
        
        /// <summary>
        /// Do not allow web pages to run javascript
        /// </summary>
        [OptionFlag("-n")]
        public bool? IsJavaScriptDisabled { get; set; }
        
        /// <summary>
        /// Minimum font size.
        /// </summary>
        [OptionFlag("--minimum-font-size")]
        public int? MinimumFontSize { get; set; }
        
        /// <summary>
        /// Use a proxy
        /// </summary>
        [OptionFlag("-p")]
        public string Proxy { get; set; }
        
        /// <summary>
        /// HTTP Authentication username.
        /// </summary>
        [OptionFlag("--username")]
        public string UserName { get; set; }

        /// <summary>
        /// HTTP Authentication password.
        /// </summary>
        [OptionFlag("--password")]
        public string Password { get; set; }
        
        /// <summary>
        /// Use this if you need another switches that are not currently supported by Rotativa.
        /// </summary>
        [OptionFlag("")]
        public string CustomSwitches { get; set; }

        public override string ToString()
        {
            var result = new StringBuilder();

            FieldInfo[] fields = GetType().GetFields();
            foreach (FieldInfo fi in fields)
            {
                var of = fi.GetCustomAttributes(typeof(OptionFlag), true).FirstOrDefault() as OptionFlag;
                if (of == null) continue;

                object value = fi.GetValue(this);
                if (value != null) result.AppendFormat(CultureInfo.InvariantCulture, " {0} {1}", of.Name, value);
            }

            return result.ToString().Trim();
        }
    }
}
