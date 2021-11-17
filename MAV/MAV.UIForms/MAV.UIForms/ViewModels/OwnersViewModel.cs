using MAV.Common.Models;
using MAV.Common.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using Xamarin.Forms;

namespace MAV.UIForms.ViewModels
{
    public class OwnersViewModel : BaseViewModel
    {
        private ApiService apiService;
        private ObservableCollection<Owner> owners;
        public ObservableCollection<Owner> Owners
        {
            get { return this.owners; }
            set { this.SetValue(ref this.owners, value); }
        }
        public OwnersViewModel()
        {
            this.apiService = new ApiService();
            this.LoadOwners();
        }

        private bool isRefreshing;
        public bool IsRefreshing
        {
            get { return this.isRefreshing; }
            set { this.SetValue(ref this.isRefreshing, value); }
        }
        private async void LoadOwners()
        {
            this.IsRefreshing = true;
            var url = Application.Current.Resources["URLApi"].ToString();
            var response = await this.apiService.GetListAsync<Owner>(
                url,
                "/api",
                "/Owners",
                "bearer",
                MainViewModel.GetInstance().Token.Token);
            this.IsRefreshing = false;
            if (!response.IsSuccess)
            {
                await Application.Current.MainPage.DisplayAlert("Error", response.Message, "Aceptar");
                return;
            }
            var myOwners = (List<Owner>)response.Result;
            this.Owners = new ObservableCollection<Owner>(myOwners);
        }
    }
}
