using NPRSApp.Maui.Model;
using NPRSApp.Maui.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace NPRSApp.Maui.Views
{

    public partial class ViewReportPage : ContentPage
    {
        private readonly ViewReportViewModel _viewModel;

        public ViewReportPage(ViewReportViewModel viewModel)
        {
            InitializeComponent();
            BindingContext = _viewModel = viewModel;
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();

            await _viewModel.LoadReportsAsync();
        }
    }
}

    

