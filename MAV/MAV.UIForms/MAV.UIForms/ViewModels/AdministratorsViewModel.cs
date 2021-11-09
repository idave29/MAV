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
        public AdministratorsViewModel()
        {
            this.apiService = new ApiService();
            this.LoadAdministrators();
        }

        private async void LoadAdministrators()
        {
            var response = await this.apiService.GetListAsync<Administrator>(
                "https://mediosaudiovisualesweb.azurewebsites.net/",
                "/api",
                "/Administrators");
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
