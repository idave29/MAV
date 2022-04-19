namespace MAV.UIForms.ViewModels
{
    using MAV.Common.Models;
    using MAV.Common.Services;
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Linq;
    using Xamarin.Forms;

    public class MaterialsViewModel : BaseViewModel
    {
        private ApiService apiService;
        private List<MaterialResponse> myMaterials;
        private List<MaterialRequest> myMaterialsRequest;
        private ObservableCollection<MaterialItemViewModel> materials;
        public ObservableCollection<MaterialItemViewModel> Materials
        {
            get { return this.materials; }
            set { this.SetValue(ref this.materials, value); }
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
            var response = await this.apiService.GetListAsync<MaterialResponse>(
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
            myMaterials = (List<MaterialResponse>)response.Result;
            RefreshMaterialList();
        }
        private void RefreshMaterialList()
        {
            this.Materials = new ObservableCollection<MaterialItemViewModel>(myMaterials.Select(m => new MaterialItemViewModel
            {
                Id = m.Id,
                Brand = m.Brand,
                Label = m.Label,
                MaterialModel = m.MaterialModel,
                MaterialType = m.MaterialType,
                Name = m.Name,
                Owner = m.Owner,
                SerialNum = m.SerialNum,
                Status = m.Status,
                ImageURL = m.ImageURL,
                ImageFullPath = m.ImageFullPath,
                Function = m.Function,
            }).OrderBy(m => m.Name).ToList());
        }
    

        public void AddMaterialToList(MaterialResponse material)
        {
            this.myMaterials.Add(material);
            RefreshMaterialList();
        }

        public void UpdateMaterialInList(MaterialResponse material)
        {
            var previousMaterial = myMaterials.Where(mt => mt.Id == material.Id).FirstOrDefault();
            if (previousMaterial != null)
            {
                this.myMaterials.Remove(previousMaterial);
            }
            this.myMaterials.Add(material);
            RefreshMaterialList();
        }

        public void DeleteMaterialInList(int materialId)
        {
            var previousMaterial = myMaterials.Where(mt => mt.Id == materialId).FirstOrDefault();
            if (previousMaterial != null)
            {
                this.myMaterials.Remove(previousMaterial);
            }
            RefreshMaterialList();
        }
    }
}
