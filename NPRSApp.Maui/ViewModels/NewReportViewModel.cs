using CommunityToolkit.Mvvm.Input;
using Microsoft.Maui.Controls;
using NPRSApp.Maui.Model;
using NPRSApp.Maui.Services;
using NPRSApp.Maui.Views;
using System;
using System.Threading.Tasks;
using System.Windows.Input;

namespace NPRSApp.Maui.ViewModels;

[QueryProperty(nameof(Report), "Report")]
public partial class NewReportViewModel : BaseViewModel
{
    private readonly DatabaseService _databaseService;

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

    public ICommand SaveCommand { get; }

    public NewReportViewModel(
        DatabaseService databaseService)
    {
        Title = "New Police Report";

        _databaseService = databaseService;

        SaveCommand = new AsyncRelayCommand(SaveReport);
    }

    // ========= LOAD FOR EDIT =========
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

        IsEditMode = true;
        Title = "Edit Police Report";

        OnPropertyChanged(string.Empty);
    }

    // ========= SAVE (CREATE / UPDATE) =========
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

        if (IsEditMode && Report != null)
        {
            // ✅ UPDATE
            Report.ReporterName = ReporterName;
            Report.ReporterPhone = ReporterPhone;
            Report.ReporterEmail = ReporterEmail;
            Report.Contact = Contact;
            Report.IncidentType = IncidentType;
            Report.IncidentDate = IncidentDate;
            Report.IncidentLocation = IncidentLocation;
            Report.Description = Description;

            await _databaseService.UpdateReportAsync(Report);

            await ShowAlertAsync("Updated", "Report updated successfully!");
        }
        else
        {
            // ✅ CREATE NEW (NO PDF HERE)
            var report = new PoliceReport
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
                Status = "Submitted",
                CreatedOn = DateTime.UtcNow
            };

            await _databaseService.AddReportAsync(report);

            // ✅ PDF REMOVED — USER MUST CLICK "Generate PDF"
            await ShowAlertAsync("Success", "Report submitted successfully!");
        }

        // ✅ GO BACK TO REPORT LIST
        await Shell.Current.GoToAsync(nameof(ReportListPage));
    }

    // ========= ALERT HELPER =========
    private static async Task ShowAlertAsync(string title, string message, string cancel = "OK")
    {
        var app = Application.Current;
        if (app?.Windows.Count > 0)
        {
            var page = app.Windows[0].Page;
            if (page != null)
                await page.DisplayAlertAsync(title, message, cancel);
        }
    }
}