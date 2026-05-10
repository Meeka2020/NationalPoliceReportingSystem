using CommunityToolkit.Mvvm.Input;
using NPRSApp.Maui.Model;
using NPRSApp.Maui.Services;
using NPRSApp.Maui.Views;
using System.Collections.ObjectModel;

namespace NPRSApp.Maui.ViewModels;

public partial class ViewReportViewModel : BaseViewModel
{
    private readonly DatabaseService _databaseService;

    public ObservableCollection<PoliceReport> Reports { get; } = [];

    public IAsyncRelayCommand<PoliceReport> ViewCommand { get; }
    public IAsyncRelayCommand<PoliceReport> EditCommand { get; }
    public IAsyncRelayCommand<PoliceReport> DeleteCommand { get; }

    public ViewReportViewModel(DatabaseService databaseService)
    {
        _databaseService = databaseService;

        Title = "View Reports";

        ViewCommand = new AsyncRelayCommand<PoliceReport>(OnViewAsync);
        EditCommand = new AsyncRelayCommand<PoliceReport>(OnEditAsync);
        DeleteCommand = new AsyncRelayCommand<PoliceReport>(OnDeleteAsync);
    }

    public async Task LoadReportsAsync()
    {
        if (IsBusy)
            return;

        try
        {
            IsBusy = true;
            Reports.Clear();

            var reports = await _databaseService.GetReportsAsync();

            foreach (var report in reports)
            {
                Reports.Add(report);
            }
        }
        catch (Exception ex)
        {
            await Shell.Current.DisplayAlertAsync("Error", ex.Message, "OK");
        }
        finally
        {
            IsBusy = false;
        }
    }

    private async Task OnViewAsync(PoliceReport? report)
    {
        if (report == null)
            return;

        try
        {
            await Shell.Current.GoToAsync(
                nameof(ViewSingleReportPage),
                new Dictionary<string, object>
                {
                    { "Report", report }
                });
        }
        catch (Exception ex)
        {
            await Shell.Current.DisplayAlertAsync("Navigation Error", ex.Message, "OK");
        }
    }

    private async Task OnEditAsync(PoliceReport? report)
    {
        if (report == null)
            return;

        try
        {
            await Shell.Current.GoToAsync(
                nameof(NewReportPage),
                new Dictionary<string, object>
                {
                    { "Report", report }
                });
        }
        catch (Exception ex)
        {
            await Shell.Current.DisplayAlertAsync("Edit Error", ex.Message, "OK");
        }
    }

    private async Task OnDeleteAsync(PoliceReport? report)
    {
        if (report == null)
            return;

        try
        {
            bool confirm = await Shell.Current.DisplayAlertAsync(
                "Delete",
                "Delete this report?",
                "Yes",
                "No");

            if (!confirm)
                return;

            await _databaseService.DeleteReportAsync(report);

            Reports.Remove(report);

            await Shell.Current.DisplayAlertAsync(
                "Deleted",
                "Report deleted successfully",
                "OK");
        }
        catch (Exception ex)
        {
            await Shell.Current.DisplayAlertAsync("Delete Error", ex.Message, "OK");
        }
    }
}