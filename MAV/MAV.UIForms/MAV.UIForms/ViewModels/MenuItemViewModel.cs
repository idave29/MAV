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
                case "AdministratorsPage":
                    MainViewModel.GetInstance().Administrators = new AdministratorsViewModel();
                    await App.Navigator.PushAsync(new AdministratorsPage());
                    break;
                case "ApplicantPage":
                    MainViewModel.GetInstance().Applicants = new ApplicantsViewModel();
                    await App.Navigator.PushAsync(new ApplicantPage());
                    break;
                case "ApplicantTypesPage":
                    MainViewModel.GetInstance().ApplicantTypes = new ApplicantTypesViewModel();
                    await App.Navigator.PushAsync(new ApplicantTypesPage());
                    break;
                case "InternsPage":
                    MainViewModel.GetInstance().Interns = new InternsViewModel();
                    await App.Navigator.PushAsync(new InternsPage());
                    break;
                case "LoanDetailsPage":
                    MainViewModel.GetInstance().LoanDetails = new LoanDetailsViewModel();
                    await App.Navigator.PushAsync(new LoanDetailsPage());
                    //MainViewModel.GetInstance().Loans = new LoansViewModel();
                    //await App.Navigator.PushAsync(new LoansPage());
                    break;
                case "LoansPage":
                    MainViewModel.GetInstance().Loans = new LoansViewModel();
                    await App.Navigator.PushAsync(new LoansPage());
                    break;
                case "MaterialsPage":
                    MainViewModel.GetInstance().Materials = new MaterialsViewModel();
                    await App.Navigator.PushAsync(new MaterialsPage());
                    break;
                case "MaterialTypesPage":
                    MainViewModel.GetInstance().MaterialTypes = new MaterialTypesViewModel();
                    await App.Navigator.PushAsync(new MaterialTypesPage());
                    break;
                case "OwnersPage":
                    MainViewModel.GetInstance().Owners = new OwnersViewModel();
                    await App.Navigator.PushAsync(new OwnersPage());
                    break;
                case "StatusesPage":
                    MainViewModel.GetInstance().Statuses = new StatusesViewModel();
                    await App.Navigator.PushAsync(new StatusesPage());
                    break;
                case "AboutPage":
                    await App.Navigator.PushAsync(new AboutPage());
                    break;
                case "SetupPage":
                    await App.Navigator.PushAsync(new SetupPage());
                    break;
                case "pruebaPage":
                    MainViewModel.GetInstance().Prueba = new PruebaViewModel();
                    await App.Navigator.PushAsync(new pruebaPage());

                    break;

                default:
                    MainViewModel.GetInstance().Login = new LoginViewModel();
                    Application.Current.MainPage = new NavigationPage(new LoginPage()); 
                    break; 

            }
        }
    }
}
