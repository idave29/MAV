namespace MAV.UIForms.ViewModels
{
    using MAV.Common.Models;
    using MAV.Common.Services;
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Linq;
    using Xamarin.Forms;
    public class LoanDetailsViewModel : BaseViewModel
    {
        private ApiService apiService;
        private List<LoanDetailsRequest> myLoanDetails;
        private ObservableCollection<LoanDetailItemViewModel> loanDetails;
        public ObservableCollection<LoanDetailItemViewModel> LoanDetails
        {
            get { return this.loanDetails; }
            set { this.SetValue(ref this.loanDetails, value); }
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
            var response = await this.apiService.GetListAsync<LoanDetailsRequest>(
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
            myLoanDetails = (List<LoanDetailsRequest>)response.Result;
            RefreshStatusesList();
        }

        private void RefreshStatusesList()
        {
            this.LoanDetails = new ObservableCollection<LoanDetailItemViewModel>
                (myLoanDetails.Select(mt => new LoanDetailItemViewModel
                {
                    Observations = mt.Observations,
                    DateTimeOut = mt.DateTimeOut,
                    DateTimeIn = mt.DateTimeIn,
                    Material = mt.Material
                }).OrderBy(mt => mt.DateTimeOut).ToList());
        }

        public void AddLoanDetailToList(LoanDetailsRequest loanDetail)
        {
            this.myLoanDetails.Add(loanDetail);
            RefreshStatusesList();
        }

        public void UpdateLoanDetailToList(LoanDetailsRequest loanDetail)
        {
            var previousLoanDetail = myLoanDetails.Where(mt => mt.Id == loanDetail.Id).FirstOrDefault();
            if (previousLoanDetail != null)
            {
                this.myLoanDetails.Remove(previousLoanDetail);
            }
            this.myLoanDetails.Add(loanDetail);
            RefreshStatusesList();
        }

        public void DeleteLoanDetailInList(int loanDetailId)
        {
            var previousLoanDetail = myLoanDetails.Where(mt => mt.Id == loanDetailId).FirstOrDefault();
            if (previousLoanDetail != null)
            {
                this.myLoanDetails.Remove(previousLoanDetail);
            }
            RefreshStatusesList();
        }
    }
}
