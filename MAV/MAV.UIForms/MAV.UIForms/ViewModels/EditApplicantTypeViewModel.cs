using GalaSoft.MvvmLight.Command;
using MAV.Common.Models;
using MAV.Common.Services;
using System;
using System.Windows.Input;
using Xamarin.Forms;

namespace MAV.UIForms.ViewModels
{
    public class EditApplicantTypeViewModel:BaseViewModel
    {
        private readonly ApiService apiService;
        public ApplicantTypeRequest ApplicantType { get; set; }

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
                "/ApplicantTypes",
                ApplicantType.Id,
                "bearer",
                MainViewModel.GetInstance().Token.Token);

            if (!response.IsSuccess)
            {
                await Application.Current.MainPage.DisplayAlert("Error", response.Message, "Aceptar");
                return;
            }
            
            MainViewModel.GetInstance().ApplicantTypes.DeleteApplicantTypeInList(ApplicantType.Id);
            this.isEnabled = true;
            this.isRunning = false;
            await App.Navigator.PopAsync();
        }

        private async void Save()
        {
            if(string.IsNullOrEmpty(this.ApplicantType.Name))
            {
                await Application.Current.MainPage.DisplayAlert("Error", "Debes introducir nombre", "Aceptar");
            }
            isEnabled = false;
            isRunning = true;
            
            var url = Application.Current.Resources["URLApi"].ToString();
            var response = await this.apiService.PutAsync(url,
                "/api",
                "/ApplicantTypes",
                ApplicantType.Id,ApplicantType,
                "bearer",
                MainViewModel.GetInstance().Token.Token);

            if (!response.IsSuccess)
            {
                await Application.Current.MainPage.DisplayAlert("Error", response.Message, "Aceptar");
                return;
            }
            var modifyApplicantType = (ApplicantTypeRequest)response.Result;
            MainViewModel.GetInstance().ApplicantTypes.UpdateApplicantTypeInList(modifyApplicantType);
            this.isEnabled = true;
            this.isRunning = false;
            await App.Navigator.PopAsync();
        }

        public EditApplicantTypeViewModel(ApplicantTypeRequest applicantType)
        {
            this.ApplicantType = applicantType;
            this.apiService = new ApiService();
            this.IsEnabled = true;
        }
    }
}
