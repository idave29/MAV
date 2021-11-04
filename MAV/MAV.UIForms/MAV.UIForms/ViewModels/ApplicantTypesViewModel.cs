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
        private ObservableCollection<ApplicantType> applicantTypes;
        public ObservableCollection<ApplicantType> ApplicantTypes
        {
            get { return this.applicantTypes; }
            set { this.SetValue(ref this.applicantTypes, value); }
        }
        public ApplicantTypesViewModel()
        {
            this.apiService = new ApiService();
            this.LoadApplicantTypes();
        }

        private async void LoadApplicantTypes()
        {
            var response = await this.apiService.GetListAsync<ApplicantType>(
                "https://mavweb1.azurewebsites.net",
                "/api",
                "/ApplicantTypes");
            if (!response.IsSuccess)
            {
                await Application.Current.MainPage.DisplayAlert("Error", response.Message, "Aceptar");
                return;
            }
            var myApplicantType = (List<ApplicantType>)response.Result;
            this.ApplicantTypes = new ObservableCollection<ApplicantType>(myApplicantType);
        }

    }
}
