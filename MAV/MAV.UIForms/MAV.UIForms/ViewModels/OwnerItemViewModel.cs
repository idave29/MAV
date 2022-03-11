using GalaSoft.MvvmLight.Command;
using MAV.Common.Models;
using MAV.UIForms.Views;
using System;
using System.Windows.Input;

namespace MAV.UIForms.ViewModels
{
    public class OwnerItemViewModel : OwnerRequest
    {
        public ICommand SelectOwnerCommand { get { return new RelayCommand(SelectOwner); } }

        private async void SelectOwner()
        {
            MainViewModel.GetInstance().EditOwner = new EditOwnerViewModel((OwnerRequest)this);
            await App.Navigator.PushAsync(new EditOwnerPage()); 
        }
    }
}
