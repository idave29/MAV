using GalaSoft.MvvmLight.Command;
using MAV.Common.Models;
using MAV.Common.Services;
using System;
using System.Windows.Input;
using Xamarin.Forms;

namespace MAV.UIForms.ViewModels
{
    public class EditStatusViewModel : BaseViewModel
    {
        private readonly ApiService apiService;
        public StatusRequest Status { get; set; }

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
            var confirm = await Application.Current.MainPage.DisplayAlert("Error", "Seguro que quieres borrarlo?", "Si", "No");
            if (!confirm)
                return;

            isEnabled = false;
            isRunning = true;

            var url = Application.Current.Resources["URLApi"].ToString();
            var response = await this.apiService.DeleteAsync(url,
                "/api",
                "/Status",
                Status.Id,
                "bearer",
                MainViewModel.GetInstance().Token.Token);

            if (!response.IsSuccess)
            {
                await Application.Current.MainPage.DisplayAlert("Error", response.Message, "Aceptar");
                return;
            }

            MainViewModel.GetInstance().Statuses.DeleteStatusInList(Status.Id);
            isEnabled = true;
            isRunning = false;
            await App.Navigator.PopAsync();
        }

        private async void Save()
        {
            if (string.IsNullOrEmpty(this.Status.Name))
            {
                await Application.Current.MainPage.DisplayAlert("Error", "Debes introducir nombre", "Aceptar");
            }
            isEnabled = false;
            isRunning = true;

            var url = Application.Current.Resources["URLApi"].ToString();
            var response = await this.apiService.PutAsync(url,
                "/api",
                "/Status",
                Status.Id, Status,
                "bearer",
                MainViewModel.GetInstance().Token.Token);

            if (!response.IsSuccess)
            {
                await Application.Current.MainPage.DisplayAlert("Error", response.Message, "Aceptar");
                return;
            }
            var modifyStatus = (StatusRequest)response.Result;
            MainViewModel.GetInstance().Statuses.UpdateStatusToList(modifyStatus);
            isEnabled = true;
            isRunning = false;
            await App.Navigator.PopAsync();
        }

        public EditStatusViewModel(StatusRequest status)
        {
            this.Status = status;
            this.apiService = new ApiService();
            this.IsEnabled = true;
        }
    }
}

