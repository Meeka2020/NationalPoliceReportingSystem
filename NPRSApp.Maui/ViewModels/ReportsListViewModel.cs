using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using NPRSApp.Maui.Model;
using NPRSApp.Maui.Services;
using NPRSApp.Maui.Views;
using System.Collections.ObjectModel;

namespace NPRSApp.Maui.ViewModels;

public partial class ReportListViewModel : ObservableObject
{
    private readonly DatabaseService _databaseService;

    public ObservableCollection<PoliceReport> Reports { get; } = [];

    public IRelayCommand AddReportCommand { get; }

    public ReportListViewModel(DatabaseService databaseService)
    {
        _databaseService = databaseService;

        AddReportCommand = new RelayCommand(NavigateToNewReport);

        // ✅ SAFE async call
        _ = LoadReportsAsync();
    }

    private async Task LoadReportsAsync()
    {
        var items = await _databaseService.GetReportsAsync();
        Reports.Clear();

        foreach (var item in items)
            Reports.Add(item);
    }

    private async void NavigateToNewReport()
    {
        await Shell.Current.GoToAsync(nameof(NewReportPage));
    }
}