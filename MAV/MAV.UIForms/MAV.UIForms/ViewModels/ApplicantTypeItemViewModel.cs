using GalaSoft.MvvmLight.Command;
using MAV.Common.Models;
using MAV.UIForms.Views;
using System;
using System.Windows.Input;

namespace MAV.UIForms.ViewModels
{
    public class ApplicantTypeItemViewModel:ApplicantTypeRequest
    {
        public ICommand SelectApplicantTypeCommand { get { return new RelayCommand(SelectApplicantType); } }

        private async void SelectApplicantType()
        {
            MainViewModel.GetInstance().EditApplicantType = new EditApplicantTypeViewModel((ApplicantTypeRequest)this);
            await App.Navigator.PushAsync(new EditApplicantTypePage());
        }
    }
}
