using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using NPRSApp.Maui.Model;
using NPRSApp.Maui.Services;
using NPRSApp.Maui.Views;


namespace NPRSApp.Maui.ViewModels;

public partial class ViewSingleReportViewModel : ObservableObject
{
    private PoliceReport? report;

    public PoliceReport? Report
    {
        get => report;
        set => SetProperty(ref report, value);
    }
}