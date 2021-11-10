namespace MAV.UIForms.ViewModels
{
    using MAV.Common.Models;
    using MAV.Common.Services;
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using Xamarin.Forms;
    public class LoanDetailsViewModel : BaseViewModel
    {
        private ApiService apiService;
        private ObservableCollection<LoanDetails> loanDetail;
        public ObservableCollection<LoanDetails> LoanDetails
        {
            get { return this.loanDetail; }
            set { this.SetValue(ref this.loanDetail, value); }
        }
        public LoanDetailsViewModel()
        {
            this.apiService = new ApiService();
            this.LoadLoans();
        }

        private async void LoadLoans()
        {
            var response = await this.apiService.GetListAsync<LoanDetails>(
                "https://mediosaudiovisualesweb.azurewebsites.net/",
                "/api",
                "/LoanDetails");
            if (!response.IsSuccess)
            {
                await Application.Current.MainPage.DisplayAlert("Error", response.Message, "Aceptar");
                return;
            }
            var myLoanDetail = (List<LoanDetails>)response.Result;
            this.LoanDetails = new ObservableCollection<LoanDetails>(myLoanDetail);
        }
    }
}
