using System;
using System.IO;
using System.Web;
using OfficeOpenXml;
using iText.Kernel.Pdf;
using iText.Layout;
using iText.Layout.Element;

namespace MovieTicketBooking.Others
{
    public class ExcelPdfPrinter
    {
        public void CreateExcelFile(string filePath)
        {
            using (var package = new ExcelPackage())
            {
                var worksheet = package.Workbook.Worksheets.Add("Sheet1");

                // Add some data to the Excel file
                worksheet.Cells["A1"].Value = "Hello";
                worksheet.Cells["B1"].Value = "World";

                package.SaveAs(new FileInfo(filePath));
            }
        }

        public void PrintPdfFromExcel(string excelFilePath, string pdfFilePath)
        {
            // Load the Excel file using EPPlus
            using (var excelPackage = new ExcelPackage(new FileInfo(excelFilePath)))
            {
                // Create a new PDF document using iTextSharp
                using (var pdfDocument = new PdfDocument(new PdfWriter(pdfFilePath)))
                {
                    var document = new Document(pdfDocument);

                    // Access the first worksheet in the Excel file
                    var worksheet = excelPackage.Workbook.Worksheets[0];

                    // Loop through all rows and columns in the worksheet
                    for (int rowNum = 1; rowNum <= worksheet.Dimension.Rows; rowNum++)
                    {
                        for (int colNum = 1; colNum <= worksheet.Dimension.Columns; colNum++)
                        {
                            // Get the cell value from the Excel worksheet
                            var cellValue = worksheet.Cells[rowNum, colNum].Text;

                            // Add the cell value to the PDF document
                            document.Add(new Paragraph(cellValue));
                        }
                    }
                }
            }
        }

        public void GenerateAndPrintPdf()
        {
            string excelFilePath = HttpContext.Current.Server.MapPath("~/Files/YourExcelFile.xlsx");
            string pdfFilePath = HttpContext.Current.Server.MapPath("~/Files/YourPdfFile.pdf");

            // Generate PDF from Excel
            PrintPdfFromExcel(excelFilePath, pdfFilePath);

            // You can implement PDF printing logic here, for example, using a PDF viewer/printing library
            // For web applications, you might provide a download link or display the PDF in an iframe
        }
    }
}
