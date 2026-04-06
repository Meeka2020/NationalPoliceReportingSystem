using NPRSApp.Maui.Views;

namespace NPRSApp.Maui;

public partial class AppShell : Shell
{
    public AppShell()
    {
        InitializeComponent();

        // ✅ Register navigation routes here
        Routing.RegisterRoute(nameof(NewReportPage), typeof(NewReportPage));
        Routing.RegisterRoute(nameof(ReportListPage), typeof(ReportListPage));
        Routing.RegisterRoute(nameof(ViewReportPage), typeof(ViewReportPage));
    }
}