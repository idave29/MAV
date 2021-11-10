namespace MAV.UIForms.ViewModels
{
    using MAV.Common.Models;
    using MAV.Common.Services;
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using Xamarin.Forms;
    public class MaterialsViewModel : BaseViewModel
    {
        private ApiService apiService;
        private ObservableCollection<Material> material;
        public ObservableCollection<Material> Materials
        {
            get { return this.material; }
            set { this.SetValue(ref this.material, value); }
        }
        public MaterialsViewModel()
        {
            this.apiService = new ApiService();
            this.LoadMaterials();
        }

        private async void LoadMaterials()
        {
            var response = await this.apiService.GetListAsync<Material>(
                "https://mediosaudiovisualesweb.azurewebsites.net/",
                "/api",
                "/Materials");
            if (!response.IsSuccess)
            {
                await Application.Current.MainPage.DisplayAlert("Error", response.Message, "Aceptar");
                return;
            }
            var myMaterial = (List<Material>)response.Result;
            this.Materials = new ObservableCollection<Material>(myMaterial);
        }
    }
}
