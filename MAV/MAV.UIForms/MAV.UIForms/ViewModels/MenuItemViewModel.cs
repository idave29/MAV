namespace MAV.UIForms.ViewModels
{
    using GalaSoft.MvvmLight.Command;
    using MAV.UIForms.Views;
    using System.Windows.Input;
    using Xamarin.Forms;

    public class MenuItemViewModel : MAV.Common.Models.Menu 
    {
        public ICommand SelectMenuCommand { get { return new RelayCommand(SelectMenu); } }

        private async void SelectMenu()
        {
            App.Master.IsPresented = false; 
            //var mainviewmodel = MainViewModel.GetInstance();
            switch (this.PageName)
            {
                case "AboutPage":
                    await App.Navigator.PushAsync(new AboutPage());
                    break;
                case "SetupPage":
                    await App.Navigator.PushAsync(new SetupPage());
                    break;
                default:
                    MainViewModel.GetInstance().Login = new LoginViewModel();
                    Application.Current.MainPage = new NavigationPage(new LoginPage()); 
                    break; 
            }
        }
    }
}
