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
        private ObservableCollection<MaterialRequest> material;
        public ObservableCollection<MaterialRequest> Materials
        {
            get { return this.material; }
            set { this.SetValue(ref this.material, value); }
        }

        private bool isRefreshing;
        public bool IsRefreshing
        {
            get { return this.isRefreshing; }
            set { this.SetValue(ref this.isRefreshing, value); }
        }
        public MaterialsViewModel()
        {
            this.apiService = new ApiService();
            this.LoadMaterials();
        }

        private async void LoadMaterials()
        {
            this.IsRefreshing = true;
            var url = Application.Current.Resources["URLApi"].ToString();
            var response = await this.apiService.GetListAsync<MaterialRequest>(
                url,
                "/api",
                "/Materials",
                "bearer",
                MainViewModel.GetInstance().Token.Token);
            this.IsRefreshing = false;
            if (!response.IsSuccess)
            {
                await Application.Current.MainPage.DisplayAlert("Error", response.Message, "Aceptar");
                return;
            }
            var myMaterial = (List<MaterialRequest>)response.Result;
            this.Materials = new ObservableCollection<MaterialRequest>(myMaterial);
        }
    }
}
