using GalaSoft.MvvmLight.Command;
using MAV.Common.Models;
using MAV.Common.Services;
using System;
using System.Windows.Input;
using Xamarin.Forms;

namespace MAV.UIForms.ViewModels
{
    public class AddMaterialViewModel : BaseViewModel
    {
        private readonly ApiService apiService;
        public string Name { get; set; }

        public string Label { set; get; }

        public string Brand { set; get; }

        public string MaterialModel { set; get; }

        public string SerialNum { set; get; }

        public string Status { get; set; }

        public string MaterialType { set; get; }

        public string Owner { get; set; }

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
            if (string.IsNullOrEmpty(Name))
            {
                await Application.Current.MainPage.DisplayAlert("Error", "Debes introducir un material", "Aceptar");
                return;
            }
            if (string.IsNullOrEmpty(Label))
            {
                await Application.Current.MainPage.DisplayAlert("Error", "Debes introducir una etiqueta", "Aceptar");
                return;
            }
            if (string.IsNullOrEmpty(Brand))
            {
                await Application.Current.MainPage.DisplayAlert("Error", "Debes introducir una marca", "Aceptar");
                return;
            }
            if (string.IsNullOrEmpty(MaterialModel))
            {
                await Application.Current.MainPage.DisplayAlert("Error", "Debes introducir un modelo", "Aceptar");
                return;
            }
            if (string.IsNullOrEmpty(SerialNum))
            {
                await Application.Current.MainPage.DisplayAlert("Error", "Debes introducir un numero de serie", "Aceptar");
                return;
            }
            if (string.IsNullOrEmpty(Status))
            {
                await Application.Current.MainPage.DisplayAlert("Error", "Debes introducir un status", "Aceptar");
                return;
            }
            if (string.IsNullOrEmpty(MaterialType))
            {
                await Application.Current.MainPage.DisplayAlert("Error", "Debes introducir un tipo de material", "Aceptar");
                return;
            }
            if (string.IsNullOrEmpty(Owner))
            {
                await Application.Current.MainPage.DisplayAlert("Error", "Debes introducir un dueño", "Aceptar");
                return;
            }

            isEnabled = false;
            isRunning = true;
            var material = new MaterialRequest { Name = Name, Label = Label, Brand = Brand, MaterialModel = MaterialModel, SerialNum = SerialNum, Status = Status, MaterialType = MaterialType, Owner = Owner };
            var url = Application.Current.Resources["URLApi"].ToString();
            var response = await this.apiService.PostAsync(url,
                "/api",
                "/Materials",
                material,
                "bearer",
                MainViewModel.GetInstance().Token.Token);

            if (!response.IsSuccess)
            {
                await Application.Current.MainPage.DisplayAlert("Error", response.Message, "Aceptar");
                return;
            }
            var newMaterial = (MaterialRequest)response.Result;
            MainViewModel.GetInstance().Materials.Materials.Add(newMaterial);
            isEnabled = true;
            isRunning = false;
            await App.Navigator.PopAsync();
        }

        public AddMaterialViewModel()
        {
            this.apiService = new ApiService();
            isEnabled = true;
        }
    }
}
