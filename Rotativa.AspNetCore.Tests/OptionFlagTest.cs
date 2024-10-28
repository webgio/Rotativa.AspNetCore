using System;
using System.Collections.Generic;
using System.Configuration;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Rotativa.AspNetCore.Tests
{

    [Trait("Rotativa.AspNetCore", "testing if the option flags are passed to wkhtmltopdf/wkhtmltoimage in the right way.")]
    public class OptionFlagTest
    {
        StringBuilder verificationErrors;

        public OptionFlagTest()
        {
            verificationErrors = new StringBuilder();
        }

        [Fact(DisplayName = "should not pass options by default.")]
        public void NoOptions_ShouldNotPassOptions()
        {
            var test = new ViewAsPdf();

            Assert.Empty(test.GetConvertOptions());
        }

        [Fact(DisplayName = "zoom option flag is outputted in wkhtml format.")]
        public void SingleOption_Zoom_ShouldBeFormatted()
        {
            var test = new ViewAsPdf()
            {
                Zoom = 1.5
            };

            Assert.Equal("--zoom 1.5", test.GetConvertOptions());
        }

        [Fact(DisplayName = "boolean option flag are outputted in wkhtml format.")]
        public void SingleOption_Boolean_ShouldBeFormatted()
        {
            var test = new ViewAsPdf()
            {
                NoImages = true
            };

            Assert.Equal("--no-images", test.GetConvertOptions());
        }

        [Fact(DisplayName = "multiple option flags should be combined to one option string.")]
        public void MultipleOption_Boolean_ShouldBeCombined()
        {
            var test = new ViewAsPdf()
            {
                IsLowQuality = true,
                NoImages = true
            };

            Assert.Equal("-l --no-images", test.GetConvertOptions());
        }

        [Fact(DisplayName = "dictionary options should be repeated for each key")]
        public void DictionaryOption_ShouldBeFormatted()
        {
            var test = new ViewAsPdf()
            {
                CustomHeaders = new Dictionary<string, string>
                {
                    { "Header1", "value" },
                    { "Header2", "value" },
                }
            };

            Assert.Equal("--custom-header Header1 value --custom-header Header2 value", test.GetConvertOptions());
        }
    }
}
