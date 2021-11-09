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
        public StatusesViewModel()
        {
            this.apiService = new ApiService();
            this.LoadStatuses();
        }

        private async void LoadStatuses()
        {
            var response = await this.apiService.GetListAsync<Status>(
                "https://mediosaudiovisualesweb.azurewebsites.net/",
                "/api",
                "/Status");
            if(!response.IsSuccess)
            {
                await Application.Current.MainPage.DisplayAlert("Error", response.Message, "Aceptar");
                return;
            }
            var myStatuses = (List<Status>)response.Result;
            this.Statuses = new ObservableCollection<Status>(myStatuses);
        }
    }
}
