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
    /*private async void OnSelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        // ✅ Use index directly instead of LINQ
        if (e.CurrentSelection.Count > 0 &&
            e.CurrentSelection[0] is PoliceReport selectedReport)
        {
            await Shell.Current.GoToAsync(
                nameof(ViewReportPage),
                new Dictionary<string, object>
                {
                        { "Report", selectedReport }
                });
        }

        // ✅ Clear selection so the same item can be tapped again
        if (sender is CollectionView collectionView)
        {
            collectionView.SelectedItem = null;
        }
    }*/
    private async void OnSelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        if (e.CurrentSelection.Count == 0)
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

        ((CollectionView)sender).SelectedItem = null;
    }
}
