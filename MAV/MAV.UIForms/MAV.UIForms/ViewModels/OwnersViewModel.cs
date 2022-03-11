using MAV.Common.Models;
using MAV.Common.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using Xamarin.Forms;

namespace MAV.UIForms.ViewModels
{
    public class OwnersViewModel : BaseViewModel
    {
        private ApiService apiService;
        private List<OwnerRequest> myOwners;
        private ObservableCollection<OwnerItemViewModel> owners;
        public ObservableCollection<OwnerItemViewModel> Owners
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
            var response = await this.apiService.GetListAsync<OwnerRequest>(
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
            myOwners = (List<OwnerRequest>)response.Result;
            RefreshOwnersList(); 
        }

        private void RefreshOwnersList()
        {
            this.Owners = new ObservableCollection<OwnerItemViewModel>(myOwners.Select(ow => new OwnerItemViewModel
            {
                Id = ow.Id,
                FirstName = ow.FirstName,
                LastName = ow.LastName,
                Email = ow.Email,
                //Materials = ow.Materials,
                Password = ow.Password, 
                PhoneNumber = ow.PhoneNumber
            }).OrderBy(ow => ow.FirstName).ToList());
        }

        public void AddOwnerToList(OwnerRequest owner)
        {
            this.myOwners.Add(owner);
            RefreshOwnersList(); 
        }

        public void UpdateOwnerInList(OwnerRequest owner)
        {
            var previousOwner = myOwners.Where(ow => ow.Id == owner.Id).FirstOrDefault();
            if(previousOwner != null)
            {
                this.myOwners.Remove(previousOwner);
            }
            this.myOwners.Add(owner);
            RefreshOwnersList();
        }

        public void DeleteOwnerInList(int ownerId)
        {
            var previousOwner = myOwners.Where(ow => ow.Id == ownerId).FirstOrDefault();
            if (previousOwner != null)
            {
                this.myOwners.Remove(previousOwner);
            }
            RefreshOwnersList();
        }

    }
}
