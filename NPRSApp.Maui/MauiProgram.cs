using Microsoft.Extensions.Logging;
using NPRSApp.Maui.Services;
using NPRSApp.Maui.ViewModels;
using NPRSApp.Maui.Views;

namespace NPRSApp.Maui;

public static class MauiProgram
{
	public static MauiApp CreateMauiApp()
	{
		var builder = MauiApp.CreateBuilder();
		builder
			.UseMauiApp<App>()
			.ConfigureFonts(fonts =>
			{
				fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
				fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
			});

#if DEBUG
		builder.Logging.AddDebug();
#endif
        // ADD SERVICES HERE
        builder.Services.AddSingleton<DatabaseService>();
        builder.Services.AddSingleton<NewReportViewModel>();
        builder.Services.AddTransient<NewReportPage>();
        builder.Services.AddSingleton<BookAppointmentViewModel>();
        builder.Services.AddTransient<BookAppointmentPage>(); 
        builder.Services.AddSingleton<ReportsListViewModel>();
        builder.Services.AddSingleton<ReportListPage>();
        builder.Services.AddTransient<ViewReportViewModel>();
        builder.Services.AddTransient<ViewReportPage>();
        builder.Services.AddTransient<ViewSingleReportViewModel>();
        builder.Services.AddTransient<ViewSingleReportPage>();



        return builder.Build();
	}
}
