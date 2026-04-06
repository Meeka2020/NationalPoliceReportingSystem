using NPRSApp.Maui.Model;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using Microsoft.Maui.Storage;
using Microsoft.Maui.ApplicationModel;
using System;
using System.IO;
using System.Threading.Tasks;

namespace NPRSApp.Maui.Services
{
    public class PdfService
    {
        public PdfService()
        {
            QuestPDF.Settings.License = LicenseType.Community;
        }

        public static async Task GeneratePdfAsync(PoliceReport report)
        {
            string fileName = $"PoliceReport_{report.ReportNo}.pdf";
            string filePath = Path.Combine(FileSystem.AppDataDirectory, fileName);

            await Task.Run(() =>
            {
                Document.Create(container =>
                {
                    container.Page(page =>
                    {
                        page.Margin(30);

                        page.Content().Column(col =>
                        {
                            // 🔷 Header
                            col.Item().Text("POLICE REPORT RECEIPT")
                                      .FontSize(20)
                                      .Bold()
                                      .AlignCenter();

                            col.Item().LineHorizontal(1);

                            // 🔷 Report Info
                            col.Item().Text($"Report No: {report.ReportNo}");
                            col.Item().Text($"Date Created: {report.CreatedOn:yyyy-MM-dd HH:mm}");

                            col.Item().LineHorizontal(1);

                            // 🔷 Reporter Info
                            col.Item().Text("Reporter Details").Bold();
                            col.Item().Text($"Name: {report.ReporterName}");
                            col.Item().Text($"Phone: {report.ReporterPhone}");
                            col.Item().Text($"Email: {report.ReporterEmail}");

                            col.Item().LineHorizontal(1);

                            // 🔷 Incident Info
                            col.Item().Text("Incident Details").Bold();
                            col.Item().Text($"Type: {report.IncidentType}");
                            col.Item().Text($"Date: {report.IncidentDate:yyyy-MM-dd}");
                            col.Item().Text($"Location: {report.IncidentLocation}");
                            col.Item().Text($"Description: {report.Description}");

                            col.Item().LineHorizontal(1);

                            // 🔷 Consultation (only if exists)
                            if (report.ConsultationDate != null)
                            {
                                col.Item().Text("Consultation Booking").Bold();
                                col.Item().Text($"Date: {report.ConsultationDate:yyyy-MM-dd}");
                                col.Item().Text($"Time: {report.ConsultationTime}");
                            }

                            col.Item().LineHorizontal(1);

                            // 🔷 Footer
                            col.Item().Text("Status: Submitted");
                            col.Item().Text("Thank you. Your report has been received.")
                                      .Italic()
                                      .FontSize(10);
                        });
                    });
                })
                .GeneratePdf(filePath);
            });

            // ✅ OPEN PDF AFTER GENERATION
            await Launcher.OpenAsync(new OpenFileRequest
            {
                File = new ReadOnlyFile(filePath)
            });
        }
    }
}
