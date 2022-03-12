namespace MAV.UIForms.ViewModels
{
    using MAV.Common.Models;
    using MAV.Common.Services;
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Linq;
    using Xamarin.Forms;

    public class StatusesViewModel : BaseViewModel
    {
        private ApiService apiService;
        private List<StatusRequest> myStatuses;
        private ObservableCollection<StatusItemViewModel> statuses;
        public ObservableCollection<StatusItemViewModel> Statuses
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
            var response = await this.apiService.GetListAsync<StatusRequest>(
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
            myStatuses = (List<StatusRequest>)response.Result;
            RefreshStatusesList();
        }

        private void RefreshStatusesList()
        {
            this.Statuses = new ObservableCollection<StatusItemViewModel>
                (myStatuses.Select(mt => new StatusItemViewModel
                {
                    Id = mt.Id,
                    Name = mt.Name
                }).OrderBy(mt => mt.Name).ToList());
        }

        public void AddStatusToList(StatusRequest status)
        {
            this.myStatuses.Add(status);
            RefreshStatusesList();
        }

        public void UpdateStatusToList(StatusRequest status)
        {
            var previousStatus = myStatuses.Where(mt => mt.Id == status.Id).FirstOrDefault();
            if (previousStatus != null)
            {
                this.myStatuses.Remove(previousStatus);
            }
            this.myStatuses.Add(status);
            RefreshStatusesList();
        }

        public void DeleteStatusInList(int statusId)
        {
            var previousStatus = myStatuses.Where(mt => mt.Id == statusId).FirstOrDefault();
            if (previousStatus != null)
            {
                this.myStatuses.Remove(previousStatus);
            }
            RefreshStatusesList();
        }
    }
}

