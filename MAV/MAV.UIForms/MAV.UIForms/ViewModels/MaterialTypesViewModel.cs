namespace MAV.UIForms.ViewModels
{
    using MAV.Common.Models;
    using MAV.Common.Services;
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using Xamarin.Forms;
    public class MaterialTypesViewModel : BaseViewModel
    {
        private ApiService apiService;
        private ObservableCollection<MaterialType> materialTypes;
        public ObservableCollection<MaterialType> MaterialTypes
        {
            get { return this.materialTypes; }
            set { this.SetValue(ref this.materialTypes, value); }
        }
        public MaterialTypesViewModel()
        {
            this.apiService = new ApiService();
            this.LoadMaterialTypes();
        }

        private async void LoadMaterialTypes()
        {
            var response = await this.apiService.GetListAsync<MaterialType>(
                "https://mavweb1.azurewebsites.net",
                "/api",
                "/MaterialType");
            if (!response.IsSuccess)
            {
                await Application.Current.MainPage.DisplayAlert("Error", response.Message, "Aceptar");
                return;
            }
            var myMaterialType = (List<MaterialType>)response.Result;
            this.MaterialTypes = new ObservableCollection<MaterialType>(myMaterialType);
        }
    }
}
