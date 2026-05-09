using NPRSApp.Maui.ViewModels;
using NPRSApp.Maui.Views; // ✅ ADD THIS

namespace NPRSApp.Maui;

public partial class MainPage : ContentPage
{
    public MainPage(NewReportViewModel newReportViewModel)
    {
        InitializeComponent();
        BindingContext = newReportViewModel;
    }

    private async void OnFileReportTapped(object? sender, TappedEventArgs e)
    {
        await Shell.Current.GoToAsync(nameof(NewReportPage));
    }

    private async void OnBookAppointmentTapped(object? sender, TappedEventArgs e)
    {
        await Shell.Current.GoToAsync(nameof(BookAppointmentPage));
    }

        private async void OnViewReportsTapped(object? sender, TappedEventArgs e)
        {
            await Shell.Current.GoToAsync(nameof(ViewReportPage));
        }
}