using GalaSoft.MvvmLight.Command;
using MAV.Common.Models;
using MAV.UIForms.Views;
using System;
using System.Windows.Input;

namespace MAV.UIForms.ViewModels
{
    public class LoanDetailItemViewModel : LoanDetailsRequest
    {
        public ICommand SelectLoanDetailCommand { get { return new RelayCommand(SelectLoanDetail); } }

        private async void SelectLoanDetail()
        {
            MainViewModel.GetInstance().EditLoanDetail = new EditLoanDetailViewModel((LoanDetailsRequest)this);
            await App.Navigator.PushAsync(new EditLoanDetailPage());
        }
    }
}
