using NPRSApp.Maui.Model;
using NPRSApp.Maui.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace NPRSApp.Maui.Views;

public partial class ReportListPage : ContentPage
{
    public ReportListPage(ReportListViewModel vm)
    {
        InitializeComponent();
        BindingContext = vm;
    }

    private async void OnSelectionChanged(
        object? sender,
        SelectionChangedEventArgs e)
    {
        if (e.CurrentSelection == null || e.CurrentSelection.Count == 0)
            return;

        if (e.CurrentSelection[0] is PoliceReport report)
        {
            await Shell.Current.GoToAsync(
                nameof(ViewReportPage),
                new Dictionary<string, object>
                {
                    { "Report", report }
                });
        }

        if (sender is CollectionView collectionView)
        {
            collectionView.SelectedItem = null;
        }
    }
}
