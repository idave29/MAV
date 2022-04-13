using MAV.Common.Models;
using MAV.Common.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using Xamarin.Forms;

namespace MAV.UIForms.ViewModels
{
    public class HistoryLoanPageViewModel:BaseViewModel
    {
        private readonly ApiService apiService;
        public LoanRequest Loan { get; set; }
        private List<LoanDetailsRequest> myLoansD;
        private ObservableCollection<LoanDetailItemViewModel> loansDetails;
        public ObservableCollection<LoanDetailItemViewModel> LoansDetails
        {
            get { return this.loansDetails; }
            set { this.SetValue(ref this.loansDetails, value); }
        }

        private bool isRefreshing;
        public bool IsRefreshing
        {
            get { return this.isRefreshing; }
            set { this.SetValue(ref this.isRefreshing, value); }
        }

        public HistoryLoanPageViewModel(LoanRequest loan)
        {
            this.Loan = loan;
            this.apiService = new ApiService();
            LoadLoans();
        }

        public void RefreshLoanDList()
        {

            this.LoansDetails = new ObservableCollection<LoanDetailItemViewModel>(Loan.LoanDetails.Select(l => new LoanDetailItemViewModel
            {
                Id = l.Id,
                DateTimeIn = l.DateTimeIn,
                DateTimeOut = l.DateTimeOut,
                Material = l.Material,
                Observations = l.Observations,
                Status = l.Status
            }).ToList());
        }

        private async void LoadLoans()
        {
            this.IsRefreshing = true;
            var url = Application.Current.Resources["URLApi"].ToString();
            var response = await this.apiService.GetListAsync<LoanDetailsRequest>(
                url,
                "/api",
                "/LoanDetails",
                "bearer",
                MainViewModel.GetInstance().Token.Token);
            this.IsRefreshing = false;
            if (!response.IsSuccess)
            {
                await Application.Current.MainPage.DisplayAlert("Error", response.Message, "Aceptar");
                return;
            }

            myLoansD = ((List<LoanDetailsRequest>)response.Result).ToList();
            RefreshLoanDList();
        }

        //public override void OnNavigatedto(INavigationPageController controller)
        //{
        //    base.OnPropertyChanged(controller);

        //    //if()
        //}

        public void AddLoanDetailsToList(LoanDetailsRequest loanDetail)
        {
            this.myLoansD.Add(loanDetail);
            RefreshLoanDList();
        }
        public void UpdateLoanDetailsInList(LoanDetailsRequest loanDetail)
        {
            var previousLoan = myLoansD.Where(a => a.Id == loanDetail.Id).FirstOrDefault();
            if (previousLoan != null)
            {
                this.myLoansD.Remove(previousLoan);
            }
            this.myLoansD.Add(loanDetail);
            RefreshLoanDList();
        }
        public void DeleteLoanDetailsInList(int loanDetailId)
        {
            var previousLoan = myLoansD.Where(mt => mt.Id == loanDetailId).FirstOrDefault();
            if (previousLoan != null)
            {
                this.myLoansD.Remove(previousLoan);
            }
            RefreshLoanDList();
        }
    }
}
