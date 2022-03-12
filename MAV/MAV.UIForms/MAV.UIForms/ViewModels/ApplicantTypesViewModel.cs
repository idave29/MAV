namespace MAV.UIForms.ViewModels
{
    using MAV.Common.Models;
    using MAV.Common.Services;
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Linq;
    using Xamarin.Forms;

    public class ApplicantTypesViewModel : BaseViewModel
    {
        private ApiService apiService;
        private List<ApplicantTypeRequest> myApplicantTypes;
        private ObservableCollection<ApplicantTypeItemViewModel> applicantTypes;
        public ObservableCollection<ApplicantTypeItemViewModel> ApplicantTypes
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
            myApplicantTypes = (List<ApplicantTypeRequest>)response.Result;
            RefreshApplicantTypesList();
        }

        private void RefreshApplicantTypesList()
        {
            this.ApplicantTypes = new ObservableCollection<ApplicantTypeItemViewModel>
                (myApplicantTypes.Select(at => new ApplicantTypeItemViewModel
                {
                    Id = at.Id,
                    Name = at.Name
                }).OrderBy(at => at.Name).ToList());
        }

        public void AddApplicantTypeToList(ApplicantTypeRequest applicantType)
        {
            this.myApplicantTypes.Add(applicantType);
            RefreshApplicantTypesList();
        }

        public void UpdateApplicantTypeInList(ApplicantTypeRequest applicantType)
        {
            var previousApplicantType = myApplicantTypes.Where(at => at.Id == applicantType.Id).FirstOrDefault();
            if (previousApplicantType != null)
            {
                this.myApplicantTypes.Remove(previousApplicantType);
            }
            this.myApplicantTypes.Add(applicantType);
            RefreshApplicantTypesList();
        }

        public void DeleteApplicantTypeInList(int applicantTypeId)
        {
            var previousApplicantType = myApplicantTypes.Where(at => at.Id == applicantTypeId).FirstOrDefault();
            if (previousApplicantType != null)
            {
                this.myApplicantTypes.Remove(previousApplicantType);
            }

            RefreshApplicantTypesList();
        }
    }
}
