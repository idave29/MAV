using GalaSoft.MvvmLight.Command;
using MAV.Common.Models;
using MAV.Common.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;

namespace MAV.UIForms.ViewModels
{
    public class AddAdministratorViewModel : BaseViewModel
    {
        private readonly ApiService apiService;

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
        private ObservableCollection<MAV.Common.Models.User> users;
        private MAV.Common.Models.User user;


        public MAV.Common.Models.User UserRequest
        {
            get => this.user;
            set => this.SetValue(ref this.user, value);
        }

        public ObservableCollection<MAV.Common.Models.User> Users
        {
            get => this.users;
            set => this.SetValue(ref this.users, value);
        }

        private async void LoadUsers()
        {
            var url = Application.Current.Resources["URLApi"].ToString();
            var response = await this.apiService.GetListAsync<MAV.Common.Models.User>(
                url,
                "/api",
                "/Account",
                "bearer",
                MainViewModel.GetInstance().Token.Token);

            if (!response.IsSuccess)
            {
                await Application.Current.MainPage.DisplayAlert("Error", response.Message, "Aceptar");
                return;
            }
            var myUsers = ((List<MAV.Common.Models.User>)response.Result);
            this.Users = new ObservableCollection<MAV.Common.Models.User>(myUsers);

        }


        public ICommand SaveCommand { get { return new RelayCommand(Save); } }

        private async void Save()
        {
            if (this.UserRequest == null)
            {
                await Application.Current.MainPage.DisplayAlert("Error", "Debes seleccionar un usuario", "Aceptar");
                return;
            }

            isEnabled = false;
            isRunning = true;
            var administrator = new MAV.Common.Models.AdministratorRequest
            {
                Email = UserRequest.Email,
            };
            var url = Application.Current.Resources["URLApi"].ToString();
            var response = await this.apiService.PostAsync(url,
                "/api",
                "/Administrators/",
                administrator,
                "bearer",
                MainViewModel.GetInstance().Token.Token);

            if (!response.IsSuccess)
            {
                await Application.Current.MainPage.DisplayAlert("Error", response.Message, "Aceptar");
                return;
            }

            MainViewModel.GetInstance().Administrators.LoadNewAdministrators();
            isEnabled = true;
            isRunning = false;
            await App.Navigator.PopAsync();
        }

        public AddAdministratorViewModel()
        {
            this.apiService = new ApiService();
            isEnabled = true;
            this.LoadUsers(); 
        }
    }
}