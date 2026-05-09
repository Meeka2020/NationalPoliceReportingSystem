using NPRSApp.Maui.Model;
using NPRSApp.Maui.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace NPRSApp.Maui.Views
{
    public partial class ViewReportPage : ContentPage
    {
        public ViewReportPage(ViewReportViewModel vm)
        {
            InitializeComponent();
            BindingContext = vm;
        }
        

    }
}

    

