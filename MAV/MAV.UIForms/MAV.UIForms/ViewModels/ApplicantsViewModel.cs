using System.Collections.Generic;

namespace MAV.UIForms.ViewModels
{
    using MAV.Common.Models;
    using MAV.Common.Services;
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Linq;
    using System.Text;
    using Xamarin.Forms;

    public class ApplicantsViewModel : BaseViewModel
    {
        private ApiService apiService;
        private List<ApplicantRequest> myApplicants;
        private ObservableCollection<ApplicantItemViewModel> applicants;
        public ObservableCollection<ApplicantItemViewModel> Applicants
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
            var response = await this.apiService.GetListAsync<ApplicantRequest>(
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
            myApplicants = (List<ApplicantRequest>)response.Result;
            RefreshApplicantList();

        }
        public void RefreshApplicantList()
        {
            this.Applicants = new ObservableCollection<ApplicantItemViewModel>(myApplicants.Select(a => new ApplicantItemViewModel
            {
                Id = a.Id,
                FirstName = a.FirstName,
                LastName = a.LastName,
                Email = a.Email,
                ApplicantType = a.ApplicantType,

            }).OrderBy(a => a.FirstName).ToList());
        }
        public void AddApplicantToList(ApplicantRequest applicant)
        {
            this.myApplicants.Add(applicant);
            RefreshApplicantList();
        }

        public void UpdateApplicantInList(ApplicantRequest applicant)
        {
            var previousApplicant = myApplicants.Where(a => a.Id == applicant.Id).FirstOrDefault();
            if (previousApplicant != null)
            {
                this.myApplicants.Remove(previousApplicant);
            }
            this.myApplicants.Add(applicant);
            RefreshApplicantList();
        }
        public void DeleteApplicantInList(int applicantId)
        {
            var previousApplicant = myApplicants.Where(mt => mt.Id == applicantId).FirstOrDefault();
            if (previousApplicant != null)
            {
                this.myApplicants.Remove(previousApplicant);
            }
            RefreshApplicantList();
        }
    }
}
