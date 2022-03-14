
using GalaSoft.MvvmLight.Command;
using MAV.Common.Models;
using MAV.UIForms.Views;
using System.Windows.Input;

namespace MAV.UIForms.ViewModels
{
    public class ApplicantItemViewModel : ApplicantRequest
    {
        public ICommand SelectApplicantCommand { get { return new RelayCommand(SelectApplicant); } }

        private async void SelectApplicant()
        {
            MainViewModel.GetInstance().EditApplicant = new EditApplicantViewModel((ApplicantRequest)this);
            await App.Navigator.PushAsync(new EditApplicantPage());
        }
    }
}
