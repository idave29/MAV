namespace MAV.UIForms.ViewModels
{
    using MAV.Common.Models;
    using MAV.Common.Services;
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using Xamarin.Forms;
    public class InternsViewModel: BaseViewModel
    {
        private ApiService apiService;
        private ObservableCollection<Intern> interns;
        public ObservableCollection<Intern> Interns
        {
            get { return this.interns; }
            set { this.SetValue(ref this.interns, value); }
        }
        public InternsViewModel()
        {
            this.apiService = new ApiService();
            this.LoadInterns();
        }

        private async void LoadInterns()
        {
            var response = await this.apiService.GetListAsync<Intern>(
                "https://mavweb1.azurewebsites.net",
                "/api",
                "/Interns");
            if (!response.IsSuccess)
            {
                await Application.Current.MainPage.DisplayAlert("Error", response.Message, "Aceptar");
                return;
            }
            var myIntern = (List<Intern>)response.Result;
            this.Interns = new ObservableCollection<Intern>(myIntern);
        }

    }

}
