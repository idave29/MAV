using GalaSoft.MvvmLight.Command;
using MAV.Common.Models;
using MAV.Common.Services;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;

namespace MAV.UIForms.ViewModels
{
    public class AddAdministratorViewModel : BaseViewModel
    {
        private readonly ApiService apiService;

        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }

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
                await Application.Current.MainPage.DisplayAlert("Error", "Debes introducir el nombre", "Aceptar");
                return;
            }
            if (string.IsNullOrEmpty(LastName))
            {
                await Application.Current.MainPage.DisplayAlert("Error", "Debes introducir los apellidos", "Aceptar");
                return;
            }
            if (string.IsNullOrEmpty(Email))
            {
                await Application.Current.MainPage.DisplayAlert("Error", "Debes introducir un correo", "Aceptar");
                return;
            }
            if (string.IsNullOrEmpty(PhoneNumber))
            {
                await Application.Current.MainPage.DisplayAlert("Error", "Debes introducir un telefono", "Aceptar");
                return;
            }

            isEnabled = false;
            isRunning = true;
            var administrator = new AdministratorRequest 
            {
                FirstName = FirstName,
                LastName = LastName,
                Email = Email,
                PhoneNumber = PhoneNumber,
                Password = "123456"
            };
            var url = Application.Current.Resources["URLApi"].ToString();
            var response = await this.apiService.PostAsync(url,
                "/api",
                "/Administrators",
                administrator,
                "bearer",
                MainViewModel.GetInstance().Token.Token);

            if (!response.IsSuccess)
            {
                await Application.Current.MainPage.DisplayAlert("Error", response.Message, "Aceptar");
                return;
            }
            var newAdministrator = (AdministratorRequest)response.Result;
            MainViewModel.GetInstance().Administrators.AddAdministratorToList(newAdministrator);
            isEnabled = true;
            isRunning = false;
            await App.Navigator.PopAsync();
        }

        public AddAdministratorViewModel()
        {
            this.apiService = new ApiService();
            isEnabled = true;
        }
    }
}