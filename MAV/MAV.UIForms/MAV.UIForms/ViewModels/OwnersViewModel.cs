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

        private async void LoadOwners()
        {
            var response = await this.apiService.GetListAsync<Owner>(
                "https://mediosaudiovisualesweb.azurewebsites.net/",
                "/api",
                "/Owners");
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
