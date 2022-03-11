namespace MAV.UIForms.ViewModels
{
    using MAV.Common.Models;
    using MAV.Common.Services;
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Linq;
    using Xamarin.Forms;
    public class MaterialTypesViewModel : BaseViewModel
    {
        private ApiService apiService;
        private List<MaterialTypeRequest> myMaterialTypes;
        private ObservableCollection<MaterialTypeItemViewModel> materialTypes;
        public ObservableCollection<MaterialTypeItemViewModel> MaterialTypes
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
            myMaterialTypes = (List<MaterialTypeRequest>)response.Result;
            RefreshMaterialTypesList();
        }

        private void RefreshMaterialTypesList()
        {
            this.MaterialTypes = new ObservableCollection<MaterialTypeItemViewModel>(myMaterialTypes.Select(mt => new MaterialTypeItemViewModel
            {
                Id = mt.Id,
                Name = mt.Name
            }).OrderBy(mt => mt.Name).ToList());
        }

        public void AddMaterialTypeToList(MaterialTypeRequest materialType)
        {
            this.myMaterialTypes.Add(materialType);
            RefreshMaterialTypesList();
        }

        public void UpdateMaterialTypeInList(MaterialTypeRequest materialType)
        {
            var previousMaterialType = myMaterialTypes.Where(mt => mt.Id == materialType.Id).FirstOrDefault();
            if (previousMaterialType != null)
            {
                this.myMaterialTypes.Remove(previousMaterialType);
            }
            this.myMaterialTypes.Add(materialType);
            RefreshMaterialTypesList();
        }

        public void DeleteMaterialTypeInList(int materialTypeId)
        {
            var previousMaterialType = myMaterialTypes.Where(mt => mt.Id == materialTypeId).FirstOrDefault();
            if (previousMaterialType != null)
            {
                this.myMaterialTypes.Remove(previousMaterialType);
            }
            RefreshMaterialTypesList();
        }

    }
}
