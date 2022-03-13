using GalaSoft.MvvmLight.Command;
using MAV.Common.Models;
using MAV.Common.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Input;
using Xamarin.Forms;
namespace MAV.UIForms.ViewModels
{
    public class EditInternViewModel : BaseViewModel
    {
        private readonly ApiService apiService;
        public InternRequest Intern { get; set; }

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
        public ICommand DeleteCommand { get { return new RelayCommand(Delete); } }

        private async void Delete()
        {
            var confirm = await Application.Current.MainPage.DisplayAlert("Confirmar", "¿Seguro que quieres borrarlo?", "Si", "No");
            if (!confirm)
                return;

            isEnabled = false;
            isRunning = true;

            var url = Application.Current.Resources["URLApi"].ToString();
            var response = await this.apiService.DeleteAsync(url,
                "/api",
                "/Interns",
                Intern.Id,
                "bearer",
                MainViewModel.GetInstance().Token.Token);

            if (!response.IsSuccess)
            {
                await Application.Current.MainPage.DisplayAlert("Error", response.Message, "Aceptar");
                return;
            }
            MainViewModel.GetInstance().Interns.DeleteInternInList(Intern.Id);
            this.isEnabled = true;
            this.isRunning = false;
            await App.Navigator.PopAsync();
        }

        private async void Save()
        {
            if (string.IsNullOrEmpty(this.Intern.FirstName))
            {
                await Application.Current.MainPage.DisplayAlert("Error", "Debes introducir el nombre", "Aceptar");
                return;
            }
            if (string.IsNullOrEmpty(this.Intern.LastName))
            {
                await Application.Current.MainPage.DisplayAlert("Error", "Debes introducir los apellidos", "Aceptar");
                return;
            }
            if (string.IsNullOrEmpty(this.Intern.Email))
            {
                await Application.Current.MainPage.DisplayAlert("Error", "Debes introducir un correo", "Aceptar");
                return;
            }
            if (string.IsNullOrEmpty(this.Intern.PhoneNumber))
            {
                await Application.Current.MainPage.DisplayAlert("Error", "Debes introducir un telefono", "Aceptar");
                return;
            }
            isEnabled = false;
            isRunning = true;
            var url = Application.Current.Resources["URLApi"].ToString();
            var response = await this.apiService.PutAsync(url,
                "/api",
                "/Interns",
                Intern.Id,
                Intern,
                "bearer",
                MainViewModel.GetInstance().Token.Token);

            if (!response.IsSuccess)
            {
                await Application.Current.MainPage.DisplayAlert("Error", response.Message, "Aceptar");
                return;
            }
            var modifyMaterial = (InternRequest)response.Result;
            MainViewModel.GetInstance().Interns.UpdateInternInList(modifyMaterial);
            this.isEnabled = true;
            this.isRunning = false;
            await App.Navigator.PopAsync();
        }
        public EditInternViewModel(InternRequest intern)
        {
            this.Intern = intern;
            this.apiService = new ApiService();
            this.IsEnabled = true;

        }
    }
}
