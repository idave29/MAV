using MAV.Common.Models;
using MAV.Common.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xamarin.Forms;

namespace MAV.UIForms.ViewModels
{
    public class PruebaViewModel : BaseViewModel
    {
        private IList<string> materialList;
        public IList<string> MaterialList
        {
            get { return this.materialList; }
            set { this.SetValue(ref this.materialList, value); }
        }

        private ApiService apiService;
        
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
            MaterialList = ((List<MaterialTypeRequest>)response.Result).Select(m=>m.Name).ToList();
            
        }
        public PruebaViewModel()
        {
            this.apiService = new ApiService();
            this.LoadMaterialTypes();
        }
    }
}
