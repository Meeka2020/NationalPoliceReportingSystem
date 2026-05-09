using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Maui.Controls;
using NPRSApp.Maui.Services;
using NPRSApp.Maui.ViewModels;


namespace NPRSApp.Maui.Views;

public partial class BookAppointmentPage : ContentPage
{
    public BookAppointmentPage(BookAppointmentViewModel vm)
    {
        InitializeComponent();
        BindingContext = vm;
    }

    private async void OnBackClicked(object? sender, TappedEventArgs e)
    {
        await Navigation.PopAsync();
    }
}
