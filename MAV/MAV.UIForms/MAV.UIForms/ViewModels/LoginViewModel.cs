using GalaSoft.MvvmLight.Command;
using MAV.UIForms.Views;
using System;
using System.Windows.Input;
using Xamarin.Forms;

namespace MAV.UIForms.ViewModels
{
    public class LoginViewModel
    {
        public string Email { get; set; }
        public string Password { get; set; }
        public ICommand LoginCommand { get { return new RelayCommand(Login); } }

        public LoginViewModel()
        {
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
            if (!this.Email.Equals("eduardo.fong@gmail.com")||!this.Password.Equals("123456"))
            {
                await Application.Current.MainPage.DisplayAlert("Error", "Email o contraseña incorrecta", "Aceptar");
                return;
            }
            //await Application.Current.MainPage.DisplayAlert("OK", "Liiiisto", "Aceptar");
            //return;
            MainViewModel.GetInstance().Statuses = new StatusesViewModel();
            await Application.Current.MainPage.Navigation.PushAsync(new StatusesPage());

            MainViewModel.GetInstance().Owners = new OwnersViewModel();
            await Application.Current.MainPage.Navigation.PushAsync(new OwnersPage());

            MainViewModel.GetInstance().MaterialTypes = new MaterialTypesViewModel();
            await Application.Current.MainPage.Navigation.PushAsync(new MaterialTypesPage());

            MainViewModel.GetInstance().ApplicantTypes = new ApplicantTypesViewModel();
            await Application.Current.MainPage.Navigation.PushAsync(new ApplicantTypesPage());


        }
    }
}
