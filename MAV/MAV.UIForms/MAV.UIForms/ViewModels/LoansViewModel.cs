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

        private async void LoadLoans()
        {
            var response = await this.apiService.GetListAsync<Loan>(
                "https://mediosaudiovisualesweb.azurewebsites.net/",
                "/api",
                "/Loans");
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
