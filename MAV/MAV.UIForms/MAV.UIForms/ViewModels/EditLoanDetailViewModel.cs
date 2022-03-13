using GalaSoft.MvvmLight.Command;
using MAV.Common.Models;
using MAV.Common.Services;
using System;
using System.Windows.Input;
using Xamarin.Forms;

namespace MAV.UIForms.ViewModels
{
    public class EditLoanDetailViewModel : BaseViewModel
    {
        private readonly ApiService apiService;
        public LoanDetailsRequest LoanDetail { get; set; }

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
                "/Status",
                LoanDetail.Id,
                "bearer",
                MainViewModel.GetInstance().Token.Token);

            if (!response.IsSuccess)
            {
                await Application.Current.MainPage.DisplayAlert("Error", response.Message, "Aceptar");
                return;
            }

            MainViewModel.GetInstance().Statuses.DeleteStatusInList(LoanDetail.Id);
            this.isEnabled = true;
            this.isRunning = false;
            await App.Navigator.PopAsync();
        }

        private async void Save()
        {
            if (string.IsNullOrEmpty(this.LoanDetail.Observations))
            {
                await Application.Current.MainPage.DisplayAlert("Error", "Debes introducir una observación", "Aceptar");
            }
            if (string.IsNullOrEmpty(Convert.ToString(this.LoanDetail.DateTimeOut)))
            {
                await Application.Current.MainPage.DisplayAlert("Error", "Debes una fecha y hora de salida", "Aceptar");
            }
            if (string.IsNullOrEmpty(Convert.ToString(this.LoanDetail.DateTimeIn)))
            {
                await Application.Current.MainPage.DisplayAlert("Error", "Debes una fecha y hora de entrega", "Aceptar");
            }
            if (string.IsNullOrEmpty(Convert.ToString(this.LoanDetail.Material)))
            {
                await Application.Current.MainPage.DisplayAlert("Error", "Debes introducir un material", "Aceptar");
            }
            isEnabled = false;
            isRunning = true;
            var url = Application.Current.Resources["URLApi"].ToString();
            var response = await this.apiService.PutAsync(url,
                "/api",
                "/Status",
                LoanDetail.Id,
                LoanDetail,
                "bearer",
                MainViewModel.GetInstance().Token.Token);

            if (!response.IsSuccess)
            {
                await Application.Current.MainPage.DisplayAlert("Error", response.Message, "Aceptar");
                return;
            }
            var modifyLoanDetail = (LoanDetailsRequest)response.Result;
            MainViewModel.GetInstance().LoanDetails.UpdateLoanDetailToList(modifyLoanDetail);
            this.isEnabled = true;
            this.isRunning = false;
            await App.Navigator.PopAsync();
        }

        public EditLoanDetailViewModel(LoanDetailsRequest loanDetail)
        {
            this.LoanDetail = loanDetail;
            this.apiService = new ApiService();
            this.IsEnabled = true;
        }
    }
}
