using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows.Input;
using Microsoft.Maui.Controls;

namespace NPRSApp.Maui.ViewModels;

public partial class BookAppointmentViewModel : INotifyPropertyChanged
{
    private DateTime _selectedDate = DateTime.Today;
    private TimeSpan _selectedTime = DateTime.Now.TimeOfDay;
    private string? _reason; // ✅ nullable

    public DateTime SelectedDate
    {
        get => _selectedDate;
        set
        {
            if (_selectedDate != value)
            {
                _selectedDate = value;
                OnPropertyChanged();
            }
        }
    }

    public TimeSpan SelectedTime
    {
        get => _selectedTime;
        set
        {
            if (_selectedTime != value)
            {
                _selectedTime = value;
                OnPropertyChanged();
            }
        }
    }

    public string? Reason
    {
        get => _reason;
        set
        {
            if (_reason != value)
            {
                _reason = value;
                OnPropertyChanged();
            }
        }
    }

    public ICommand BookAppointmentCommand { get; }

    public BookAppointmentViewModel()
    {
        BookAppointmentCommand =
            new Command(async () => await BookAppointmentAsync());
    }

    private async Task BookAppointmentAsync()
    {
        if (string.IsNullOrWhiteSpace(Reason))
        {
            await Shell.Current.DisplayAlertAsync(
                "Validation Error",
                "Please enter a reason for the appointment.",
                "OK");
            return;
        }

        await Shell.Current.DisplayAlertAsync(
            "Success",
            $"Appointment booked on {SelectedDate:dd MMM yyyy} at {SelectedTime:hh\\:mm}",
            "OK");
    }

    public event PropertyChangedEventHandler? PropertyChanged; // ✅ nullable

    protected void OnPropertyChanged(
        [CallerMemberName] string? name = null) // ✅ nullable
    {
        PropertyChanged?.Invoke(
            this,
            new PropertyChangedEventArgs(name));
    }
}
