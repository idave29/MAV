namespace MAV.UIForms.ViewModels
{
    using MAV.Common.Models;
    using MAV.Common.Services;
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Linq;
    using Xamarin.Forms;
    public class AdministratorsViewModel : BaseViewModel
    {
        private ApiService apiService;
        private List<AdministratorRequest> myAdmins;
        private ObservableCollection<AdministratorItemViewModel> administrators;
        public ObservableCollection<AdministratorItemViewModel> Administrators
        {
            get { return this.administrators; }
            set { this.SetValue(ref this.administrators, value); }
        }

        public AdministratorsViewModel()
        {
            this.apiService = new ApiService();
            this.LoadAdministrators();
        }

        private bool isRefreshing;
        public bool IsRefreshing
        {
            get { return this.isRefreshing; }
            set { this.SetValue(ref this.isRefreshing, value); }
        }

        private async void LoadAdministrators()
        {
            this.IsRefreshing = true;
            var url = Application.Current.Resources["URLApi"].ToString();
            var response = await this.apiService.GetListAsync<AdministratorRequest>(
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
            myAdmins = (List<AdministratorRequest>)response.Result;
            RefreshAdministratorsList();
        }

        private void RefreshAdministratorsList()
        {
            this.Administrators = new ObservableCollection<AdministratorItemViewModel>
                (myAdmins.Select(ad => new AdministratorItemViewModel
                {
                    Id = ad.Id,
                    FirstName = ad.FirstName,
                    LastName = ad.LastName,
                    Email = ad.Email,
                    PhoneNumber = ad.PhoneNumber,
                    //Password = ad.Password
                     
                }).OrderBy(ad => ad.FirstName).ToList());
        }

        public void AddAdministratorToList(AdministratorRequest administrator)
        {
            this.myAdmins.Add(administrator);
            RefreshAdministratorsList();
        }

        public void UpdateAdministratorInList(AdministratorRequest administrator)
        {
            var previousAdministrator = myAdmins.Where(ad => ad.Id == administrator.Id).FirstOrDefault();
            if (previousAdministrator != null)
            {
                this.myAdmins.Remove(previousAdministrator);
            }
            this.myAdmins.Add(administrator);
            RefreshAdministratorsList();
        }

        public void DeleteAdministratorInList(int administratorId)
        {
            var previousAdministrator = myAdmins.Where(ad => ad.Id == administratorId).FirstOrDefault();
            if (previousAdministrator != null)
            {
                this.myAdmins.Remove(previousAdministrator);
            }

            RefreshAdministratorsList();
        }
    }
}
