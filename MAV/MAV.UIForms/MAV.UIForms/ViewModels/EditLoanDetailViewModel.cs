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
    public class EditLoanDetailViewModel : BaseViewModel
    {
        private readonly ApiService apiService;
        public LoanDetailsRequest LoanDetails { get; set; }
        public MaterialResponse Materials { get; set; }

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

        private ImageSource imageSource;
        public ImageSource ImageSource
        {
            get => imageSource;
            set => this.SetValue(ref this.imageSource, value);
        }


        //public ICommand SaveCommand { get { return new RelayCommand(Save); } }
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
                "/LoanDetails",
                LoanDetails.Id,
                "bearer",
                MainViewModel.GetInstance().Token.Token);

            if (!response.IsSuccess)
            {
                await Application.Current.MainPage.DisplayAlert("Error", response.Message, "Aceptar");
                return;
            }

            MainViewModel.GetInstance().LoanDetails.DeleteLoanDetailInList(LoanDetails.Id);
            this.isEnabled = true;
            this.isRunning = false;
            await App.Navigator.PopAsync();
        }

        //private async void Save()
        //{
        //    if (string.IsNullOrEmpty(Convert.ToString(this.LoanDetails.Material.Status == 1)))
        //    {
        //        await Application.Current.MainPage.DisplayAlert("Error", "Este prestamo ya se ha devuelto", "Aceptar");
        //    }
        //    isEnabled = false;
        //    isRunning = true;
        //    var url = Application.Current.Resources["URLApi"].ToString();
        //    var response = await this.apiService.PutAsync(url,
        //        "/api",
        //        "/LoanDetails",
        //        LoanDetails.Id,
        //        LoanDetails,
        //        "bearer",
        //        MainViewModel.GetInstance().Token.Token);

        //    if (!response.IsSuccess)
        //    {
        //        await Application.Current.MainPage.DisplayAlert("Error", response.Message, "Aceptar");
        //        return;
        //    }
        //    var modifyLoanDetail = (LoanDetailsRequest)response.Result;
        //    MainViewModel.GetInstance().HistorysLoan.UpdateLoanDetailsInList(modifyLoanDetail);
        //    this.isEnabled = true;
        //    this.isRunning = false;
        //    await App.Navigator.PopAsync();
        //}

        public EditLoanDetailViewModel(LoanDetailsRequest loanDetail)
        {
            this.apiService = new ApiService();
            this.IsEnabled = true;
            this.LoanDetails = loanDetail;
            this.ImageSource = loanDetail.Material.ImageURL;
        }
    }
}
