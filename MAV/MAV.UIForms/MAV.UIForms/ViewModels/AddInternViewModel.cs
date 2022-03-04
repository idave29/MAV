using GalaSoft.MvvmLight.Command;
using MAV.Common.Models;
using MAV.Common.Services;
using System;
using System.Windows.Input;
using Xamarin.Forms;
using System.Collections.Generic;

namespace MAV.UIForms.ViewModels
{
    public class AddInternViewModel : BaseViewModel
    {
        private readonly ApiService apiService;
        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Email { get; set; }

        public string PhoneNumber { get; set; }

        public ICollection<LoanRequest> Loans { get; set; }

        private bool isRunning;
        public bool IsRunning
        {
            get { return isRunning; }
            set { this.SetValue(ref this.isRunning, value); }
        }

        private bool isEnabled;
        public bool IsEnabled
        {
            get { return isEnabled; }
            set { this.SetValue(ref this.isEnabled, value); }
        }

        public ICommand SaveCommand { get { return new RelayCommand(Save); } }

        private async void Save()
        {
            if (string.IsNullOrEmpty(FirstName))
            {
                await Application.Current.MainPage.DisplayAlert("Error", "Debes introducir un nombre", "Aceptar");
                return;
            }
            if (string.IsNullOrEmpty(LastName))
            {
                await Application.Current.MainPage.DisplayAlert("Error", "Debes introducir un apellido", "Aceptar");
                return;
            }
            if (string.IsNullOrEmpty(Email))
            {
                await Application.Current.MainPage.DisplayAlert("Error", "Debes introducir un email", "Aceptar");
                return;
            }
            if (string.IsNullOrEmpty(PhoneNumber))
            {
                await Application.Current.MainPage.DisplayAlert("Error", "Debes introducir un numero de telefono", "Aceptar");
                return;
            }
            if (Loans == null)
            {
                await Application.Current.MainPage.DisplayAlert("Error", "Debes introducir un loan", "Aceptar");
                return;
            }

            isEnabled = false;
            isRunning = true;
            var intern = new InternRequest { FirstName = FirstName, LastName = LastName, Email = Email, PhoneNumber = PhoneNumber, Loans = Loans };
            var url = Application.Current.Resources["URLApi"].ToString();
            var response = await this.apiService.PostAsync(url,
                "/api",
                "/Interns",
                intern,
                "bearer",
                MainViewModel.GetInstance().Token.Token);

            if (!response.IsSuccess)
            {
                await Application.Current.MainPage.DisplayAlert("Error", response.Message, "Aceptar");
                return;
            }
            var newIntern = (InternRequest)response.Result;
            MainViewModel.GetInstance().Interns.Interns.Add(newIntern);
            isEnabled = true;
            isRunning = false;
            await App.Navigator.PopAsync();
        }
        public AddInternViewModel()
        {
            this.apiService = new ApiService();
            isEnabled = true;
        }
    }
}
