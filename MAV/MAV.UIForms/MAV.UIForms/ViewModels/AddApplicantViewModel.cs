using GalaSoft.MvvmLight.Command;
using MAV.Common.Models;
using MAV.Common.Services;
using System.Collections.Generic;
using System.Windows.Input;
using Xamarin.Forms;


namespace MAV.UIForms.ViewModels
{
    public class AddApplicantViewModel: BaseViewModel
    {
        private readonly ApiService apiService;
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public List<string> TypeList { get; set; }

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
            var applicant = new ApplicantRequest { FirstName = FirstName, LastName = LastName, Email = Email, PhoneNumber = PhoneNumber };
            var url = Application.Current.Resources["URLApi"].ToString();
            var response = await this.apiService.PostAsync(url,
                "/api",
                "/Applicants",
                applicant,
                "bearer",
                MainViewModel.GetInstance().Token.Token);

            if (!response.IsSuccess)
            {
                await Application.Current.MainPage.DisplayAlert("Error", response.Message, "Aceptar");
                return;
            }
            var newApplicant = (ApplicantRequest)response.Result;
            MainViewModel.GetInstance().Applicants.Applicants.Add(newApplicant);
            isEnabled = true;
            isRunning = false;
            await App.Navigator.PopAsync();
        }
        
        public AddApplicantViewModel()
        {
            this.apiService = new ApiService();
            isEnabled = true;
            //TypeList= 
        }
        public IList<string> CountryList
        {
            get
            {
                return new List<string> { "USA", "Germany", "England" };
            }
        }
    }
}
