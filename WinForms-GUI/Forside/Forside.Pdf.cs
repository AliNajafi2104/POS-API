using PdfiumViewer;
using System;
using System.IO;

namespace WinformsGUI
{
    public partial class Forside
    {

        PdfiumViewer.PdfViewer pdf;
        private void openPdfToolStripMenuItem_Click(object sender, EventArgs e)
        {
          

            byte[] bytes = System.IO.File.ReadAllBytes(@"C:\priser vvm pdf.pdf");
            var stream = new MemoryStream(bytes);
            PdfDocument pdfDocument = PdfiumViewer.PdfDocument.Load(stream);
            pdf.Document = pdfDocument;

            FocusButton();

        }
        private void button32_Click(object sender, EventArgs e)
        {
            FocusButton();
        }

    }
}
