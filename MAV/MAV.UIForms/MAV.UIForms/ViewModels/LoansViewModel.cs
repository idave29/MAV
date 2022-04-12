namespace MAV.UIForms.ViewModels
{
    using MAV.Common.Models;
    using MAV.Common.Services;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Linq;
    using Xamarin.Forms;
    public class LoansViewModel : BaseViewModel
    {
        private ApiService apiService;
        private List<LoanRequest> myLoans;
        private ObservableCollection<LoanItemViewModel> loans;
        public ObservableCollection<LoanItemViewModel> Loans
        {
            get { return this.loans; }
            set { this.SetValue(ref this.loans, value); }
        }
        public LoansViewModel()
        {
            this.apiService = new ApiService();
            this.LoadLoans();
        }

        private bool isRefreshing;
        public bool IsRefreshing
        {
            get { return this.isRefreshing; }
            set { this.SetValue(ref this.isRefreshing, value); }
        }
        private async void LoadLoans()
        {
            this.IsRefreshing = true;
            var url = Application.Current.Resources["URLApi"].ToString();
            var response = await this.apiService.GetListAsync<LoanRequest>(
                url,
                "/api",
                "/Loans",
                "bearer",
                MainViewModel.GetInstance().Token.Token);
            this.IsRefreshing = false;
            if (!response.IsSuccess)
            {
                await Application.Current.MainPage.DisplayAlert("Error", response.Message, "Aceptar");
                return;
            }
            myLoans = (List<LoanRequest>)response.Result;
            RefreshLoanList();
        }
        public void RefreshLoanList()
        {
            this.Loans = new ObservableCollection<LoanItemViewModel>(myLoans.Select(l => new LoanItemViewModel
            {
                Id = l.Id,
                Applicant = l.Applicant,
                Intern = l.Intern,
                LoanDetails = l.LoanDetails
            }).OrderBy(l => l.Id).ToList());
        }
        public void AddLoanToList(LoanRequest loan)
        {
            this.myLoans.Add(loan);
            RefreshLoanList();
        }

        public void UpdateLoanInList(LoanRequest loan)
        {
            var previousLoan = myLoans.Where(a => a.Id == loan.Id).FirstOrDefault();
            if (previousLoan != null)
            {
                this.myLoans.Remove(previousLoan);
            }
            this.myLoans.Add(loan);
            RefreshLoanList();
        }
        public void DeleteLoanInList(int loanId)
        {
            var previousLoan = myLoans.Where(mt => mt.Id == loanId).FirstOrDefault();
            if (previousLoan != null)
            {
                this.myLoans.Remove(previousLoan);
            }
            RefreshLoanList();
        }
    }
}
