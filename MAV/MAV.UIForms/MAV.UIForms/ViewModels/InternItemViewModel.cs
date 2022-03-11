using GalaSoft.MvvmLight.Command;
using MAV.Common.Models;
using MAV.UIForms.Views;
using System;
using System.Windows.Input;

namespace MAV.UIForms.ViewModels
{
    public class InternItemViewModel : InternRequest
    {
        public ICommand SelectInternCommand { get { return new RelayCommand(SelectIntern); } }

        private async void SelectIntern()
        {
            MainViewModel.GetInstance().EditIntern = new EditInternViewModel((InternRequest)this);
            await App.Navigator.PushAsync(new EditInternPage());
        }
    }
}
