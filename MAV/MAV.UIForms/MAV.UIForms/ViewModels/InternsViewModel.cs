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

        private bool isRefreshing;
        public bool IsRefreshing
        {
            get { return this.isRefreshing; }
            set { this.SetValue(ref this.isRefreshing, value); }
        }
        public InternsViewModel()
        {
            this.apiService = new ApiService();
            this.LoadInterns();
        }

        private async void LoadInterns()
        {
            this.IsRefreshing = true;
            var url = Application.Current.Resources["URLApi"].ToString();
            var response = await this.apiService.GetListAsync<Intern>(
                url,
                "/api",
                "/Interns",
                "bearer",
                MainViewModel.GetInstance().Token.Token);
            this.IsRefreshing = false;
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
