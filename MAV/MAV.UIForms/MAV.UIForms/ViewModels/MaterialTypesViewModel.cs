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
        private ObservableCollection<MaterialTypeRequest> materialTypes;
        public ObservableCollection<MaterialTypeRequest> MaterialTypes
        {
            get { return this.materialTypes; }
            set { this.SetValue(ref this.materialTypes, value); }
        }

        private bool isRefreshing;
        public bool IsRefreshing
        {
            get { return this.isRefreshing; }
            set { this.SetValue(ref this.isRefreshing, value); }
        }

        public MaterialTypesViewModel()
        {
            this.apiService = new ApiService();
            this.LoadMaterialTypes();
        }

        private async void LoadMaterialTypes()
        {
            this.IsRefreshing = true;
            var url = Application.Current.Resources["URLApi"].ToString();
            var response = await this.apiService.GetListAsync<MaterialTypeRequest>(
                url,
                "/api",
                "/MaterialTypes",
                "bearer",
                MainViewModel.GetInstance().Token.Token);
            this.IsRefreshing = false;
            if (!response.IsSuccess)
            {
                await Application.Current.MainPage.DisplayAlert("Error", response.Message, "Aceptar");
                return;
            }
            var myMaterialType = (List<MaterialTypeRequest>)response.Result;
            this.MaterialTypes = new ObservableCollection<MaterialTypeRequest>(myMaterialType);
        }
    }
}
