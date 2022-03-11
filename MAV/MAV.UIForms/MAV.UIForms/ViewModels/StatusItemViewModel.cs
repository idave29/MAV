using GalaSoft.MvvmLight.Command;
using MAV.Common.Models;
using MAV.UIForms.Views;
using System;
using System.Windows.Input;

namespace MAV.UIForms.ViewModels
{
    public class StatusItemViewModel : StatusRequest
    {
        public ICommand SelectStatusCommand { get { return new RelayCommand(SelectStatus); } }

        private async void SelectStatus()
        {
            MainViewModel.GetInstance().EditStatus = new EditStatusViewModel((StatusRequest)this);
            await App.Navigator.PushAsync(new EditStatusPage());
        }
    }
}
