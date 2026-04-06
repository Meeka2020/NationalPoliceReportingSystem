using NPRSApp.Maui.ViewModels;
namespace NPRSApp.Maui;

public partial class MainPage : ContentPage
{
	

	public MainPage(NewReportViewModel newReportViewModel)
	{
		InitializeComponent();
		BindingContext = newReportViewModel;
	}

	
}
