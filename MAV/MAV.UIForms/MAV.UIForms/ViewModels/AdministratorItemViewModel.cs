using GalaSoft.MvvmLight.Command;
using MAV.Common.Models;
using MAV.UIForms.Views;
using System.Windows.Input;

namespace MAV.UIForms.ViewModels
{
    public class AdministratorItemViewModel : AdministratorRequest
    {
        public ICommand SelectAdministratorCommand { get { return new RelayCommand(SelectAdministrator); } }

        private async void SelectAdministrator()
        {
            MainViewModel.GetInstance().EditAdministrator = new EditAdministratorViewModel((AdministratorRequest)this);
            await App.Navigator.PushAsync(new EditAdministratorPage());
        }
    }
}
