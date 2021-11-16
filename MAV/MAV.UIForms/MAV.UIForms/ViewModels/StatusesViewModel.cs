namespace MAV.UIForms.ViewModels
{
    using MAV.Common.Models;
    using MAV.Common.Services;
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using Xamarin.Forms;

    public class StatusesViewModel : BaseViewModel
    {
        private ApiService apiService;
        private ObservableCollection<Status> statuses;
        public ObservableCollection<Status> Statuses
        {
            get { return this.statuses; }
            set { this.SetValue(ref this.statuses, value); }
        }

        private bool isRefreshing;
        public bool IsRefreshing
        {
            get { return this.isRefreshing; }
            set { this.SetValue(ref this.isRefreshing, value); }
        }

        public StatusesViewModel()
        {
            this.apiService = new ApiService();
            this.LoadStatuses();
        }

        private async void LoadStatuses()
        {
            this.IsRefreshing = true;
            var url = Application.Current.Resources["URLApi"].ToString();
            var response = await this.apiService.GetListAsync<Status>(
                url,
                "/api",
                "/Status",
                "bearer",
                MainViewModel.GetInstance().Token.Token);
            this.IsRefreshing = false;
            if (!response.IsSuccess)
            {
                await Application.Current.MainPage.DisplayAlert("Error", response.Message, "Aceptar");
                return;
            }
            var myStatuses = (List<Status>)response.Result;
            this.Statuses = new ObservableCollection<Status>(myStatuses);
        }
    }
}
