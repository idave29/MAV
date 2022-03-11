namespace MAV.UIForms.ViewModels
{
    using GalaSoft.MvvmLight.Command;
    using MAV.Common.Models;
    using MAV.Common.Services;
    using System.Collections.Generic;
    using System.Windows.Input;
    using Xamarin.Forms;

    public class AddOwnerViewModel : BaseViewModel
    {
        private readonly ApiService apiService;
        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Email { get; set; }

        public string PhoneNumber { get; set; }

        //public ICollection<MaterialRequest> Materials { get; set; }

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
                await Application.Current.MainPage.DisplayAlert("Error", "Debes introducir un nombre", "Aceptar");
                return;
            }
            if (string.IsNullOrEmpty(LastName))
            {
                await Application.Current.MainPage.DisplayAlert("Error", "Debes introducir un apellido", "Aceptar");
                return;
            }
            if (string.IsNullOrEmpty(Email))
            {
                await Application.Current.MainPage.DisplayAlert("Error", "Debes introducir un email", "Aceptar");
                return;
            }
            if (string.IsNullOrEmpty(PhoneNumber))
            {
                await Application.Current.MainPage.DisplayAlert("Error", "Debes introducir un numero de telefono", "Aceptar");
                return;
            }

            isEnabled = false;
            isRunning = true;
            var owner = new OwnerRequest 
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
                "/Owners",
                owner,
                "bearer",
                MainViewModel.GetInstance().Token.Token);

            if (!response.IsSuccess)
            {
                await Application.Current.MainPage.DisplayAlert("Error", response.Message, "Aceptar");
                return;
            }
            var newOwner = (OwnerRequest)response.Result;
            MainViewModel.GetInstance().Owners.AddOwnerToList(newOwner);
            isEnabled = true;
            isRunning = false;
            await App.Navigator.PopAsync();
        }
        public AddOwnerViewModel()
        {
            this.apiService = new ApiService();
            isEnabled = true;
        }
    }
}
