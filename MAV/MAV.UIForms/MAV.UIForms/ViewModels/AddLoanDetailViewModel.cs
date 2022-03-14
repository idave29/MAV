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
    public class AddLoanDetailViewModel : BaseViewModel
    {
        private readonly ApiService apiService;
        public string Observations { get; set; }
        public DateTime DateTimeOut { get; set; }
        public DateTime DateTimeIn { get; set; }
        public MaterialRequest Material { get; set; }

        private bool isRunning;
        private IList<string> materialList;
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

        public IList<string> MaterialList
        {
            get { return this.materialList; }
            set { this.SetValue(ref this.materialList, value); }
        }

        public ICommand SaveCommand { get { return new RelayCommand(Save); } }

        private async void LoadMaterial()
        {
            var url = Application.Current.Resources["URLApi"].ToString();
            var response = await this.apiService.GetListAsync<MaterialRequest>(
                url,
                "/api",
                "/Materials",
                "bearer",
                MainViewModel.GetInstance().Token.Token);

            if (!response.IsSuccess)
            {
                await Application.Current.MainPage.DisplayAlert("Error", response.Message, "Aceptar");
                return;
            }
            MaterialList = ((List<MaterialRequest>)response.Result).Select(m => m.Name).ToList();

        }

        private async void Save()
        {
            if (string.IsNullOrEmpty(Observations))
            {
                await Application.Current.MainPage.DisplayAlert("Error", "Debes introducir una observación", "Aceptar");
                return;
            }
            if (string.IsNullOrEmpty(Convert.ToString(DateTimeOut)))
            {
                await Application.Current.MainPage.DisplayAlert("Error", "Debes introducir una fecha y hora de salida", "Aceptar");
                return;
            }
            if (string.IsNullOrEmpty(Convert.ToString(DateTimeIn)))
            {
                await Application.Current.MainPage.DisplayAlert("Error", "Debes introducir una fecha y hora de devolución", "Aceptar");
                return;
            }
            if (string.IsNullOrEmpty(Convert.ToString(Material)))
            {
                await Application.Current.MainPage.DisplayAlert("Error", "Debes introducir un material", "Aceptar");
                return;
            }

            isEnabled = false;
            isRunning = true;
            var loanDetail = new LoanDetailsRequest { Observations = Observations, DateTimeIn = DateTimeIn, DateTimeOut = DateTimeOut, Material = Material };
            var url = Application.Current.Resources["URLApi"].ToString();
            var response = await this.apiService.PostAsync(url,
                "/api",
                "/LoanDetails",
                loanDetail,
                "bearer",
                MainViewModel.GetInstance().Token.Token);

            if (!response.IsSuccess)
            {
                await Application.Current.MainPage.DisplayAlert("Error", response.Message, "Aceptar");
                return;
            }
            var newLoanDetail = (LoanDetailsRequest)response.Result;
            MainViewModel.GetInstance().LoanDetails.AddLoanDetailToList(newLoanDetail);
            isEnabled = true;
            isRunning = false;
            await App.Navigator.PopAsync();
        }

        public AddLoanDetailViewModel()
        {
            this.apiService = new ApiService();
            isEnabled = true;
            this.LoadMaterial();
        }
    }
}
