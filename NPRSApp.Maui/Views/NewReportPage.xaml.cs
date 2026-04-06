using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Maui.Controls;
using NPRSApp.Maui.Services;
using NPRSApp.Maui.ViewModels;

namespace NPRSApp.Maui.Views;

public partial class NewReportPage : ContentPage
{

    public NewReportPage(NewReportViewModel vm)
    {
        InitializeComponent();
        BindingContext = vm;
    }

}
