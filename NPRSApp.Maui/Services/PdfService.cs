using NPRSApp.Maui.Model;
using Microsoft.Maui.Storage;
using Microsoft.Maui.ApplicationModel;
using iText.Kernel.Pdf;
using iText.Layout;
using iText.Layout.Element;
using iText.Layout.Properties;
using iText.IO.Image;
using System.IO;
using System.Threading.Tasks;

namespace NPRSApp.Maui.Services
{
    public class PdfService
    {
        public static async Task GeneratePdfAsync(PoliceReport report)
        {
            string fileName = $"PoliceReport_{report.ReportNo}.pdf";
            string filePath = Path.Combine(FileSystem.AppDataDirectory, fileName);

            using var writer = new PdfWriter(filePath);
            using var pdf = new PdfDocument(writer);
            using var document = new Document(pdf);

            document.SetMargins(30, 30, 30, 30);

            // ===== LOGO =====
            try
            {
                using var stream =
                    await FileSystem.OpenAppPackageFileAsync(
                        "jamaica-constabulary-force_logo.png");
                using var memory = new MemoryStream();
                await stream.CopyToAsync(memory);

                var imageData = ImageDataFactory.Create(memory.ToArray());
                var logo = new iText.Layout.Element.Image(imageData)
                    .SetHorizontalAlignment(
                        iText.Layout.Properties.HorizontalAlignment.CENTER)
                    .SetMaxHeight(80);

                document.Add(logo);
            }
            catch
            {
                document.Add(new Paragraph("JCF")
                    .SetTextAlignment(
                        iText.Layout.Properties.TextAlignment.CENTER)
                    .SetFontSize(16));
            }

            // ===== HEADER =====
            document.Add(new Paragraph("JAMAICA CONSTABULARY FORCE")
                .SetFontSize(14)
                .SetTextAlignment(
                    iText.Layout.Properties.TextAlignment.CENTER));

            document.Add(new Paragraph("POLICE REPORT RECEIPT")
                .SetFontSize(18)
                .SetTextAlignment(
                    iText.Layout.Properties.TextAlignment.CENTER));

            document.Add(new Paragraph("Official Document")
                .SetFontSize(10)
                .SetTextAlignment(
                    iText.Layout.Properties.TextAlignment.CENTER));

            document.Add(new Paragraph("\n"));

            // ===== CONTENT =====
            document.Add(new Paragraph($"Report No: {report.ReportNo}"));
            document.Add(new Paragraph(
                $"Date Created: {report.CreatedOn:yyyy-MM-dd HH:mm}"));

            document.Add(new Paragraph("\n"));

            document.Add(new Paragraph("Reporter Details")
                .SetFontSize(12));
            document.Add(new Paragraph($"Name: {report.ReporterName}"));
            document.Add(new Paragraph($"Phone: {report.ReporterPhone}"));
            document.Add(new Paragraph($"Email: {report.ReporterEmail}"));

            document.Add(new Paragraph("\n"));

            document.Add(new Paragraph("Incident Details")
                .SetFontSize(12));
            document.Add(new Paragraph($"Type: {report.IncidentType}"));
            document.Add(new Paragraph(
                $"Date: {report.IncidentDate:yyyy-MM-dd}"));
            document.Add(new Paragraph(
                $"Location: {report.IncidentLocation}"));
            document.Add(new Paragraph(
                $"Description: {report.Description}"));

            if (report.ConsultationDate != null)
            {
                document.Add(new Paragraph("\n"));
                document.Add(new Paragraph("Consultation Booking")
                    .SetFontSize(12));
                document.Add(new Paragraph(
                    $"Date: {report.ConsultationDate:yyyy-MM-dd}"));
                document.Add(new Paragraph(
                    $"Time: {report.ConsultationTime}"));
            }

            // ===== STATUS =====
            document.Add(new Paragraph("\n"));
            document.Add(new Paragraph("Status: Submitted")
                .SetFontSize(12));

            document.Add(new Paragraph(
                "Thank you. Your report has been received.")
                .SetFontSize(10));

            document.Close();

            // ===== OPEN PDF =====
            await Launcher.OpenAsync(new OpenFileRequest
            {
                File = new ReadOnlyFile(filePath)
            });
        }
    }
}