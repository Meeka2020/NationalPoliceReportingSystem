using NPRSApp.Maui.Model;
using NPRSApp.Maui.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace NPRSApp.Maui.Views 
{ 
    public partial class ViewSingleReportPage : ContentPage
    {
      public ViewSingleReportPage(ViewSingleReportViewModel vm)
        {
            InitializeComponent();
            BindingContext = vm;
        }

    }
}