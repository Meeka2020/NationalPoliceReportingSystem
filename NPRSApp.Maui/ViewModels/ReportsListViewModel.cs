using CommunityToolkit.Mvvm.Input;
using NPRSApp.Maui.Model;
using NPRSApp.Maui.Services;
using NPRSApp.Maui.Views;
using System.Collections.ObjectModel;

namespace NPRSApp.Maui.ViewModels;

public partial class ReportsListViewModel : BaseViewModel
{
    private readonly DatabaseService _databaseService;

    public ObservableCollection<PoliceReport> Reports { get; } = [];

    public IAsyncRelayCommand<PoliceReport> ViewCommand { get; }
    public IAsyncRelayCommand<PoliceReport> EditCommand { get; }
    public IAsyncRelayCommand<PoliceReport> DeleteCommand { get; }

    public ReportsListViewModel(DatabaseService databaseService)
    {
        _databaseService = databaseService;
        Title = "View Reports";

        ViewCommand = new AsyncRelayCommand<PoliceReport>(OnViewAsync);
        EditCommand = new AsyncRelayCommand<PoliceReport>(OnEditAsync);
        DeleteCommand = new AsyncRelayCommand<PoliceReport>(OnDeleteAsync);
    }

    public async Task LoadReportsAsync()
    {
        if (IsBusy) return;

        try
        {
            IsBusy = true;
            Reports.Clear();

            var reports = await _databaseService.GetReportsAsync();

            foreach (var report in reports)
                Reports.Add(report);
        }
        finally
        {
            IsBusy = false;
        }
    }

    private async Task OnViewAsync(PoliceReport? report)
    {
        if (report == null) return;

        await Shell.Current.GoToAsync(
            nameof(ViewSingleReportPage),
            new Dictionary<string, object>
            {
                { "Report", report }
            });
    }

    private async Task OnEditAsync(PoliceReport? report)
    {
        if (report == null) return;

        await Shell.Current.GoToAsync(
            nameof(NewReportPage),
            new Dictionary<string, object>
            {
                { "Report", report }
            });
    }

    private async Task OnDeleteAsync(PoliceReport? report)
    {
        if (report == null) return;

        bool confirm = await Shell.Current.DisplayAlertAsync(
            "Delete",
            "Are you sure you want to delete this report?",
            "Yes",
            "No");

        if (!confirm) return;

        await _databaseService.DeleteReportAsync(report.Id);

        Reports.Remove(report);
    }
}