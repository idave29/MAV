namespace MAV.UIForms.ViewModels
{
    using MAV.Common.Models;
    using MAV.Common.Services;
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using Xamarin.Forms;
    public class AdministratorsViewModel : BaseViewModel
    {
        private ApiService apiService;
        private ObservableCollection<Administrator> administrators;
        public ObservableCollection<Administrator> Administrators
        {
            get { return this.administrators; }
            set { this.SetValue(ref this.administrators, value); }
        }

        private bool isRefreshing;
        public bool IsRefreshing
        {
            get { return this.isRefreshing; }
            set { this.SetValue(ref this.isRefreshing, value); }
        }
        public AdministratorsViewModel()
        {
            this.apiService = new ApiService();
            this.LoadAdministrators();
        }

        private async void LoadAdministrators()
        {
            this.IsRefreshing = true;
            var url = Application.Current.Resources["URLApi"].ToString();
            var response = await this.apiService.GetListAsync<Administrator>(
                url,
                "/api",
                "/Administrators",
                "bearer",
                MainViewModel.GetInstance().Token.Token);
            this.IsRefreshing = false;
            if (!response.IsSuccess)
            {
                await Application.Current.MainPage.DisplayAlert("Error", response.Message, "Aceptar");
                return;
            }
            var myAdmin = (List<Administrator>)response.Result;
            this.Administrators = new ObservableCollection<Administrator>(myAdmin);
        }
    }
}
