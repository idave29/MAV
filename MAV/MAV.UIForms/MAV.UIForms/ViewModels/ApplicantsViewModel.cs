using System.Collections.Generic;

namespace MAV.UIForms.ViewModels
{
    using MAV.Common.Models;
    using MAV.Common.Services;
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Text;
    using Xamarin.Forms;

    public class ApplicantsViewModel : BaseViewModel
    {
        private ApiService apiService;
        private ObservableCollection<Applicant> applicants;
        public ObservableCollection<Applicant> Applicants
        {
            get { return this.applicants; }
            set { this.SetValue(ref this.applicants, value); }
        }

        private bool isRefreshing;
        public bool IsRefreshing
        {
            get { return this.isRefreshing; }
            set { this.SetValue(ref this.isRefreshing, value); }
        }
        public ApplicantsViewModel()
        {
            this.apiService = new ApiService();
            this.LoadApplicants();
        }

        private async void LoadApplicants()
        {
            this.IsRefreshing = true;
            var url = Application.Current.Resources["URLApi"].ToString();
            var response = await this.apiService.GetListAsync<Applicant>(
                url,
                "/api",
                "/Applicants",
                "bearer",
                MainViewModel.GetInstance().Token.Token);
            this.IsRefreshing = false;
            if (!response.IsSuccess)
            {
                await Application.Current.MainPage.DisplayAlert("Error", response.Message, "Aceptar");
                return;
            }
            var myApplicants = (List<Applicant>)response.Result;
            this.Applicants = new ObservableCollection<Applicant>(myApplicants);

        }
    }
}
