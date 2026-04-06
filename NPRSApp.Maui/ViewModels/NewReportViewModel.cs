using CommunityToolkit.Mvvm.Input;
using Microsoft.Maui.Controls;
using NPRSApp.Maui.Model;
using NPRSApp.Maui.Services;
using System;
using System.Threading.Tasks;
using System.Windows.Input;

namespace NPRSApp.Maui.ViewModels
{
    [QueryProperty(nameof(Report), "Report")]
    public partial class NewReportViewModel : BaseViewModel
    {
        private readonly DatabaseService _databaseService;
        private readonly PdfService _pdfService;

        // ========= EDIT SUPPORT =========
        private PoliceReport? _report;
        public PoliceReport? Report
        {
            get => _report;
            set
            {
                SetProperty(ref _report, value);
                if (value != null)
                {
                    LoadReportForEdit(value);
                }
            }
        }

        public bool IsEditMode { get; private set; }

        // ========= FORM FIELDS =========
        public string ReporterName { get; set; } = string.Empty;
        public string ReporterPhone { get; set; } = string.Empty;
        public string ReporterEmail { get; set; } = string.Empty;
        public string Contact { get; set; } = string.Empty;
        public string IncidentType { get; set; } = string.Empty;
        public DateTime IncidentDate { get; set; } = DateTime.Today;
        public string IncidentLocation { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;

        // Consultation fields
        public DateTime? ConsultationDate { get; set; }
        public TimeSpan? ConsultationTime { get; set; }

        public ICommand SaveCommand { get; }

        public NewReportViewModel(
            DatabaseService databaseService,
            PdfService pdfService)
        {
            Title = "New Police Report";

            _databaseService = databaseService;
            _pdfService = pdfService;

            SaveCommand = new AsyncRelayCommand(SaveReport);
        }

        // ========= LOAD EXISTING REPORT FOR EDIT =========
        private void LoadReportForEdit(PoliceReport report)
        {
            ReporterName = report.ReporterName;
            ReporterPhone = report.ReporterPhone;
            ReporterEmail = report.ReporterEmail;
            Contact = report.Contact;
            IncidentType = report.IncidentType;
            IncidentDate = report.IncidentDate;
            IncidentLocation = report.IncidentLocation;
            Description = report.Description;
            ConsultationDate = report.ConsultationDate;
            ConsultationTime = report.ConsultationTime;

            IsEditMode = true;
            Title = "Edit Police Report";

            // Refresh entire UI
            OnPropertyChanged(string.Empty);
        }

        // ========= SAVE (CREATE OR UPDATE) =========
        private async Task SaveReport()
        {
            // ✅ VALIDATION
            if (string.IsNullOrWhiteSpace(ReporterName) ||
                string.IsNullOrWhiteSpace(IncidentType) ||
                string.IsNullOrWhiteSpace(IncidentLocation))
            {
                await ShowAlertAsync("Error", "Please fill all required fields.");
                return;
            }

            PoliceReport report;

            if (IsEditMode && Report != null)
            {
                // ✅ UPDATE EXISTING REPORT
                report = Report;

                report.ReporterName = ReporterName;
                report.ReporterPhone = ReporterPhone;
                report.ReporterEmail = ReporterEmail;
                report.Contact = Contact;
                report.IncidentType = IncidentType;
                report.IncidentDate = IncidentDate;
                report.IncidentLocation = IncidentLocation;
                report.Description = Description;
                report.ConsultationDate = ConsultationDate;
                report.ConsultationTime = ConsultationTime;

                await _databaseService.UpdateReportAsync(report);

                await ShowAlertAsync("Updated", "Report updated successfully!");
            }
            else
            {
                // ✅ CREATE NEW REPORT
                report = new PoliceReport
                {
                    ReportNo = $"PR-{DateTime.Now:yyyyMMddHHmmss}",
                    ReporterName = ReporterName,
                    ReporterPhone = ReporterPhone,
                    ReporterEmail = ReporterEmail,
                    Contact = Contact,
                    IncidentType = IncidentType,
                    IncidentDate = IncidentDate,
                    IncidentLocation = IncidentLocation,
                    Description = Description,
                    ConsultationDate = ConsultationDate,
                    ConsultationTime = ConsultationTime
                };

                await _databaseService.AddReportAsync(report);

                await PdfService.GeneratePdfAsync(report);

                await ShowAlertAsync("Success", "Report submitted successfully!");
            }

            await Shell.Current.GoToAsync(nameof(Views.ReportListPage));
        }

        // ========= ALERT HELPER =========
        private static async Task ShowAlertAsync(string title, string message, string cancel = "OK")
        {
            var app = Application.Current;

            if (app?.Windows != null && app.Windows.Count > 0)
            {
                var page = app.Windows[0].Page;
                if (page != null)
                {
                    await page.DisplayAlertAsync(title, message, cancel);
                }
            }
        }
    }
}