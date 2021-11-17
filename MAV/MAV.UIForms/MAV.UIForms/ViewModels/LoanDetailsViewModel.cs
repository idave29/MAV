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

        private bool isRefreshing;
        public bool IsRefreshing
        {
            get { return this.isRefreshing; }
            set { this.SetValue(ref this.isRefreshing, value); }
        }
        public LoanDetailsViewModel()
        {
            this.apiService = new ApiService();
            this.LoadLoans();
        }

        private async void LoadLoans()
        {
            this.IsRefreshing = true;
            var url = Application.Current.Resources["URLApi"].ToString();
            var response = await this.apiService.GetListAsync<LoanDetails>(
                url,
                "/api",
                "/Loandetails",
                "bearer",
                MainViewModel.GetInstance().Token.Token);
            this.IsRefreshing = false;
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
