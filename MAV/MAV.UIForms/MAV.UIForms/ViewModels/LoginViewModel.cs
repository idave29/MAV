using GalaSoft.MvvmLight.Command;
using MAV.Common.Models;
using MAV.Common.Services;
using MAV.UIForms.Views;
using System;
using System.Windows.Input;
using Xamarin.Forms;

namespace MAV.UIForms.ViewModels
{
    public class LoginViewModel : BaseViewModel 
    {
        private readonly ApiService apiService;

        private bool isRunning; 
        public  bool IsRunning { 
            get { return isRunning; } 
            set { this.SetValue(ref this.isRunning, value); }
        }
        private bool isEnabled; 
        public bool IsEnabled
        {
            get { return isEnabled; }
            set { this.SetValue(ref this.isEnabled, value); }
        }
        public string Email { get; set; }
        public string Password { get; set; }
        public ICommand LoginCommand { get { return new RelayCommand(Login); } }

        public LoginViewModel()
        {
            this.apiService = new ApiService();
            this.IsEnabled = true;
            this.IsRunning = false;
            this.Email = "eduardo.fong@gmail.com";
            this.Password = "123456";
        }

        private async void Login()
        {
            if(string.IsNullOrEmpty(this.Email))
            {
                await Application.Current.MainPage.DisplayAlert("Error", "Debes introducir un Email", "Aceptar");
                return;
            }
            if (string.IsNullOrEmpty(this.Password))
            {
                await Application.Current.MainPage.DisplayAlert("Error", "Debes introducir una contraseña", "Aceptar");
                return;
            }
            this.IsEnabled = false;
            this.isRunning = true;
            var request = new TokenRequest
            {
                Password = this.Password,
                Username = this.Email
            }; 
            var url = Application.Current.Resources["URLApi"].ToString();
            var response = await this.apiService.GetTokenAsync(url,
                "/Account",
                "/CreateToken",
                request);

            this.IsEnabled = true;
            this.isRunning = false;

            if(!response.IsSuccess)
            {
                await Application.Current.MainPage.DisplayAlert("Error", "Email o contraseña incorrecta", "Aceptar");
                return;
            }
            //if (!this.Email.Equals("eduardo.fong@gmail.com")||!this.Password.Equals("123456"))
            //{
            //    await Application.Current.MainPage.DisplayAlert("Error", "Email o contraseña incorrecta", "Aceptar");
            //    return;
            //}
            //await Application.Current.MainPage.DisplayAlert("OK", "Liiiisto", "Aceptar");
            //return;
            var token = (TokenResponse)response.Result; 
            var mainViewModel = MainViewModel.GetInstance();
            mainViewModel.Token = token;

            MainViewModel.GetInstance().Statuses = new StatusesViewModel();
            await Application.Current.MainPage.Navigation.PushAsync(new StatusesPage());

            MainViewModel.GetInstance().Owners = new OwnersViewModel();
            await Application.Current.MainPage.Navigation.PushAsync(new OwnersPage());

            MainViewModel.GetInstance().MaterialTypes = new MaterialTypesViewModel();
            await Application.Current.MainPage.Navigation.PushAsync(new MaterialTypesPage());

            MainViewModel.GetInstance().ApplicantTypes = new ApplicantTypesViewModel();
            await Application.Current.MainPage.Navigation.PushAsync(new ApplicantTypesPage());

            MainViewModel.GetInstance().Interns = new InternsViewModel();
            await Application.Current.MainPage.Navigation.PushAsync(new InternsPage());

            MainViewModel.GetInstance().Applicants = new ApplicantsViewModel();
            await Application.Current.MainPage.Navigation.PushAsync(new ApplicantPage());

            MainViewModel.GetInstance().Administrators = new AdministratorsViewModel();
            await Application.Current.MainPage.Navigation.PushAsync(new AdministratorsPage());

            MainViewModel.GetInstance().Materials = new MaterialsViewModel();
            await Application.Current.MainPage.Navigation.PushAsync(new MaterialsPage());

            MainViewModel.GetInstance().Loans = new LoansViewModel();
            await Application.Current.MainPage.Navigation.PushAsync(new LoansPage());

            MainViewModel.GetInstance().LoanDetails = new LoanDetailsViewModel();
            await Application.Current.MainPage.Navigation.PushAsync(new LoanDetailsPage());
        }
    }
}
