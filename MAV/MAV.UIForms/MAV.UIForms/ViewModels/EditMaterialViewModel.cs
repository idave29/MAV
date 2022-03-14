using GalaSoft.MvvmLight.Command;
using MAV.Common.Models;
using MAV.Common.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Input;
using Xamarin.Forms;

namespace MAV.UIForms.ViewModels
{
    public class EditMaterialViewModel : BaseViewModel
    {
        private readonly ApiService apiService;
        public MaterialRequest Material { get; set; }

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
        public ICommand DeleteCommand { get { return new RelayCommand(Delete); } }

        private async void Delete()
        {
            var confirm = await Application.Current.MainPage.DisplayAlert("Confirmar", "¿Seguro que quieres borrarlo", "Si", "No");
            if (!confirm)
                return;

            isEnabled = false;
            isRunning = true;

            var url = Application.Current.Resources["URLApi"].ToString();
            var response = await this.apiService.DeleteAsync(url,
                "/api",
                "/Materials",
                Material.Id,
                "bearer",
                MainViewModel.GetInstance().Token.Token);

            if (!response.IsSuccess)
            {
                await Application.Current.MainPage.DisplayAlert("Error", response.Message, "Aceptar");
                return;
            }
            MainViewModel.GetInstance().Materials.DeleteMaterialInList(Material.Id);
            this.isEnabled = true;
            this.isRunning = false;
            await App.Navigator.PopAsync();
        }

        private async void Save()
        {
            if (string.IsNullOrEmpty(this.Material.Name))
            {
                await Application.Current.MainPage.DisplayAlert("Error", "Debes introducir un nombre", "Aceptar");
                return;
            }
            isEnabled = false;
            isRunning = true;
            var url = Application.Current.Resources["URLApi"].ToString();
            var response = await this.apiService.PutAsync(url,
                "/api",
                "/Materials",
                Material.Id,
                Material,
                "bearer",
                MainViewModel.GetInstance().Token.Token);

            if (!response.IsSuccess)
            {
                await Application.Current.MainPage.DisplayAlert("Error", response.Message, "Aceptar");
                return;
            }
            var modifyMaterial = (MaterialRequest)response.Result;
            MainViewModel.GetInstance().Materials.UpdateMaterialInList(modifyMaterial);
            this.isEnabled = true;
            this.isRunning = false;
            await App.Navigator.PopAsync();
        }
        public EditMaterialViewModel(MaterialRequest material)
        {
            this.Material = material;
            this.apiService = new ApiService();
            this.IsEnabled = true;
            this.LoadMaterialTypes();
            this.LoadOwners();
            this.LoadStatuses();
        }
        private IList<string> statusList;
        private IList<string> materialTypeList;
        private IList<string> ownerList;
        public IList<string> StatusList
        {
            get { return this.statusList; }
            set { this.SetValue(ref this.statusList, value); }
        }

        public IList<string> MaterialTypeList
        {
            get { return this.materialTypeList; }
            set { this.SetValue(ref this.materialTypeList, value); }
        }

        public IList<string> OwnerList
        {
            get { return this.ownerList; }
            set { this.SetValue(ref this.ownerList, value); }
        }
        private async void LoadStatuses()
        {
            var url = Application.Current.Resources["URLApi"].ToString();
            var response = await this.apiService.GetListAsync<StatusRequest>(
                url,
                "/api",
                "/Status",
                "bearer",
                MainViewModel.GetInstance().Token.Token);

            if (!response.IsSuccess)
            {
                await Application.Current.MainPage.DisplayAlert("Error", response.Message, "Aceptar");
                return;
            }
            StatusList = ((List<StatusRequest>)response.Result).Select(m => m.Name).ToList();

        }

        private async void LoadMaterialTypes()
        {
            var url = Application.Current.Resources["URLApi"].ToString();
            var response = await this.apiService.GetListAsync<MaterialTypeRequest>(
                url,
                "/api",
                "/MaterialTypes",
                "bearer",
                MainViewModel.GetInstance().Token.Token);

            if (!response.IsSuccess)
            {
                await Application.Current.MainPage.DisplayAlert("Error", response.Message, "Aceptar");
                return;
            }
            MaterialTypeList = ((List<MaterialTypeRequest>)response.Result).Select(m => m.Name).ToList();

        }

        private async void LoadOwners()
        {
            var url = Application.Current.Resources["URLApi"].ToString();
            var response = await this.apiService.GetListAsync<OwnerRequest>(
                url,
                "/api",
                "/Owners",
                "bearer",
                MainViewModel.GetInstance().Token.Token);

            if (!response.IsSuccess)
            {
                await Application.Current.MainPage.DisplayAlert("Error", response.Message, "Aceptar");
                return;
            }
            OwnerList = ((List<OwnerRequest>)response.Result).Select(m => m.Email).ToList();

        }
    }
}
