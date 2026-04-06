using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using NPRSApp.Maui.Model;
using NPRSApp.Maui.Services;
using NPRSApp.Maui.Views;

namespace NPRSApp.Maui.ViewModels;

[QueryProperty(nameof(Report), "Report")]
public partial class ViewReportViewModel : BaseViewModel
{
    private readonly DatabaseService _databaseService;
    private readonly PdfService _pdfService;

    private PoliceReport? _report;
    public PoliceReport? Report
    {
        get => _report;
        set
        {
            SetProperty(ref _report, value);
            OnPropertyChanged(nameof(HasConsultation));
        }
    }

    public bool HasConsultation =>
        Report?.ConsultationDate != null;

    public IRelayCommand EditCommand { get; }
    public IAsyncRelayCommand DeleteCommand { get; }
    public IAsyncRelayCommand PdfCommand { get; }

    public ViewReportViewModel(
        DatabaseService databaseService,
        PdfService pdfService)
    {
        _databaseService = databaseService;
        _pdfService = pdfService;

        Title = "Report Details";

        EditCommand = new AsyncRelayCommand(OnEditAsync);
        DeleteCommand = new AsyncRelayCommand(OnDeleteAsync);
        PdfCommand = new AsyncRelayCommand(OnGeneratePdfAsync);
    }

    private async Task OnEditAsync()
    {
        if (Report is null) return;

        await Shell.Current.GoToAsync(
            nameof(NewReportPage),
            new Dictionary<string, object>
            {
                { "Report", Report }
            });
    }

    private async Task OnDeleteAsync()
    {
        if (Report is null) return;

        bool confirm = await Shell.Current.DisplayAlertAsync(
            "Confirm Delete",
            "Are you sure you want to delete this report?",
            "Yes",
            "No");

        if (!confirm)
            return;

        await _databaseService.DeleteReportAsync(Report);

        await Shell.Current.DisplayAlertAsync(
            "Deleted",
            "Report deleted successfully.",
            "OK");

        await Shell.Current.GoToAsync("..");
    }

    private async Task OnGeneratePdfAsync()
    {
        if (Report is null) return;

        await PdfService.GeneratePdfAsync(Report);

        await Shell.Current.DisplayAlertAsync(
            "PDF Generated",
            "The report receipt has been generated.",
            "OK");
    
    }
}