//using iTextSharp.text.exceptions;
//using iTextSharp.text.pdf;
//using iTextSharp.text.pdf.parser;
using iText.Kernel.Exceptions;
using iText.Kernel.Pdf;
using iText.Kernel.Pdf.Canvas.Parser;
using iText.Kernel.Pdf.Canvas.Parser.Listener;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rotativa.AspNetCore.Tests
{
    public class PdfTester
    {
        private byte[] pdfContent;
        private string pdfText;
        private PdfReader pdfReader;

        public bool PdfIsValid { get; set; }
        public Exception PdfException { get; set; }

        public void LoadPdf(byte[] pdfcontent)
        {
            try
            {
                this.pdfReader = new PdfReader(new MemoryStream(pdfcontent));
                var parser = new PDFParser();
                var parsed = parser.ExtractTextFromPDFBytes(pdfcontent);
                this.PdfIsValid = true;
            }
            catch (PdfException ex)
            {
                this.PdfException = ex;
                this.PdfIsValid = false;
            }
        }

        public bool PdfContains(string text)
        {
            var pdfDocument = new PdfDocument(this.pdfReader);

            for (int page = 1; page <= pdfDocument.GetNumberOfPages(); page++)
            {
                var strategy = new SimpleTextExtractionStrategy();
                string currentText = PdfTextExtractor.GetTextFromPage(pdfDocument.GetPage(page), strategy);

                currentText = Encoding.UTF8.GetString(ASCIIEncoding.Convert(Encoding.Default, Encoding.UTF8, Encoding.Default.GetBytes(currentText)));
                if (currentText.Contains(text))
                    return true;
                pdfReader.Close();
            }
            return false;
        }
    }
}
