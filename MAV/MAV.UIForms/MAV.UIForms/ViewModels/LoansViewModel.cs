namespace MAV.UIForms.ViewModels
{
    using MAV.Common.Models;
    using MAV.Common.Services;
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using Xamarin.Forms;
    public class LoansViewModel : BaseViewModel
    {
        private ApiService apiService;
        private ObservableCollection<Loan> loan;
        public ObservableCollection<Loan> Loans
        {
            get { return this.loan; }
            set { this.SetValue(ref this.loan, value); }
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
            var response = await this.apiService.GetListAsync<Loan>(
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
            var myLoan = (List<Loan>)response.Result;
            this.Loans = new ObservableCollection<Loan>(myLoan);
        }
    }
}
