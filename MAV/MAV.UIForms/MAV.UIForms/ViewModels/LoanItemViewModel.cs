using GalaSoft.MvvmLight.Command;
using MAV.Common.Models;
using MAV.UIForms.Views;
using System.Windows.Input;

namespace MAV.UIForms.ViewModels
{
    public class LoanItemViewModel:LoanRequest
    {
        public ICommand SelectLoanCommand { get { return new RelayCommand(SelectLoan); } }

        private async void SelectLoan()
        {
            //MainViewModel.GetInstance().EditLoan = new EditLoanViewModel((LoanRequest)this);
            //await App.Navigator.PushAsync(new EditLoanPage());

            //MainViewModel.GetInstance().LoanDetails = new LoanDetailsViewModel((LoanRequest)this);
            //await App.Navigator.PushAsync(new LoanDetailsPage());

            MainViewModel.GetInstance().HistorysLoan = new HistoryLoanPageViewModel((LoanRequest)this);
            await App.Navigator.PushAsync(new HistoryLoanPage());
        }
    }
    
}
