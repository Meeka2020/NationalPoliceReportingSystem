using CommunityToolkit.Mvvm.Input;
using Microsoft.Maui.Controls;
using NPRSApp.Maui.Model;
using NPRSApp.Maui.Services;
using System;
using System.Threading.Tasks;
using System.Windows.Input;

namespace NPRSApp.Maui.ViewModels;

[QueryProperty(nameof(ReportId), "ReportId")]
public partial class NewReportViewModel : BaseViewModel
{
    private readonly DatabaseService _databaseService;

    // ========= CONSTRUCTOR =========
    public NewReportViewModel(DatabaseService databaseService)
    {
        Title = "New Police Report";
        _databaseService = databaseService;

        SaveCommand = new AsyncRelayCommand(SaveReport);
    }

    // ========= ✅ FIXED: REPORT ID =========
    private string? reportId;
    public string ?ReportId
    {
        get => reportId;
        set
        {
            reportId = value;

            // ✅ FIX: convert string to int safely
            if (int.TryParse(value, out int id))
            {
                LoadReportById(id);
            }
        }
    }

    // ========= LOAD REPORT BY ID =========
    private async void LoadReportById(int id)
    {
        if (id <= 0)
            return;

        try
        {
            var report = await _databaseService.GetReportByIdAsync(id);

            if (report != null && report.Id > 0)
            {
                Report = report;
            }
        }
        catch (Exception ex)
        {
            await ShowAlertAsync("Load Error",
                $"{ex.Message}\n{ex.InnerException?.Message}");
        }
    }

    // ========= EDIT SUPPORT =========
    private PoliceReport? _report;
    public PoliceReport? Report
    {
        get => _report;
        set
        {
            SetProperty(ref _report, value);

            if (value != null)
                LoadReportForEdit(value);
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

    // ========= LOAD DATA INTO FORM =========
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

        // ✅ refresh UI
        OnPropertyChanged(string.Empty);
    }

    // ========= SAVE =========
    private async Task SaveReport()
    {
        if (string.IsNullOrWhiteSpace(ReporterName) ||
            string.IsNullOrWhiteSpace(IncidentType) ||
            string.IsNullOrWhiteSpace(IncidentLocation))
        {
            await ShowAlertAsync("Error", "Please fill all required fields.");
            return;
        }

        try
        {
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
                // ✅ CREATE
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

                await ShowAlertAsync("Success", "Report submitted successfully!");
            }

            // ✅ IMPORTANT FIX: go back to list after save/edit
            await Shell.Current.GoToAsync("..");
        }
        catch (Exception ex)
        {
            await ShowAlertAsync("Save Error",
                $"{ex.Message}\n{ex.InnerException?.Message}");
        }
    }

    // ========= ALERT =========
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