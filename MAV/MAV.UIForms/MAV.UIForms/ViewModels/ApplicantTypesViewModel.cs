namespace MAV.UIForms.ViewModels
{
    using MAV.Common.Models;
    using MAV.Common.Services;
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using Xamarin.Forms;

    public class ApplicantTypesViewModel : BaseViewModel
    {
        private ApiService apiService;
        private ObservableCollection<ApplicantTypeRequest> applicantTypes;
        public ObservableCollection<ApplicantTypeRequest> ApplicantTypes
        {
            get { return this.applicantTypes; }
            set { this.SetValue(ref this.applicantTypes, value); }
        }
        private bool isRefreshing; 
        public bool IsRefreshing
        {
            get { return this.isRefreshing; }
            set { this.SetValue(ref this.isRefreshing, value); }
        }
        public ApplicantTypesViewModel()
        {
            this.apiService = new ApiService();
            this.LoadApplicantTypes();
        }

        private async void LoadApplicantTypes()
        {
            this.IsRefreshing=true;
            var url = Application.Current.Resources["URLApi"].ToString();
            var response = await this.apiService.GetListAsync<ApplicantTypeRequest>(
                url,
                "/api",
                "/ApplicantTypes", 
                "bearer",
                MainViewModel.GetInstance().Token.Token);
            this.IsRefreshing = false;
            if (!response.IsSuccess)
            {
                await Application.Current.MainPage.DisplayAlert("Error", response.Message, "Aceptar");
                return;
            }
            var myApplicantType = (List<ApplicantTypeRequest>)response.Result;
            this.ApplicantTypes = new ObservableCollection<ApplicantTypeRequest>(myApplicantType);
        }

    }
}
