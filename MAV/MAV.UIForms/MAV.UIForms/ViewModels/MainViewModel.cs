namespace MAV.UIForms.ViewModels
{
    using GalaSoft.MvvmLight.Command;
    using MAV.Common.Models;
    using MAV.UIForms.Views;
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Linq;
    using System.Windows.Input;

    public class MainViewModel
    {


        public PruebaViewModel Prueba { get; set; }

        private static MainViewModel instance;
        public TokenResponse Token { get; set; }
        public LoginViewModel Login { get; set; }
        public StatusesViewModel Statuses { get; set; }
        public OwnersViewModel Owners { get; set; }
        public MaterialTypesViewModel MaterialTypes { get; set; }
        public ApplicantTypesViewModel ApplicantTypes { get; set; }
        public InternsViewModel Interns { get; set; }
        public ApplicantsViewModel Applicants { get; set; }
        public AdministratorsViewModel Administrators { get; set; }
        public MaterialsViewModel Materials { get; set; }
        public LoansViewModel Loans { get; set; }
        public LoanDetailsViewModel LoanDetails { get; set; }

        public ObservableCollection<MenuItemViewModel> Menus { get; set; }
        public AddStatusViewModel AddStatus { get; set; }
        public AddMaterialViewModel AddMaterial { get; set; }
        public AddInternViewModel AddIntern { get; set; }
        public AddOwnerViewModel AddOwner { get; set; }
        public AddMaterialTypeViewModel AddMaterialType { get; set; }
        public AddAdministratorViewModel AddAdministrator { get; set; }
        public AddApplicantTypeViewModel AddApplicantType { get; set; }
        public AddLoanDetailViewModel AddLoanDetail { get; set; }
        public AddApplicantViewModel AddApplicant { get; set; }
        public EditOwnerViewModel EditOwner { get; set; }
        public EditMaterialTypeViewModel EditMaterialType { get; set; }
        public EditStatusViewModel EditStatus { get; set; }
        public EditMaterialViewModel EditMaterial { get; set; }
        public EditInternViewModel EditIntern { get; set; }
        public EditApplicantTypeViewModel EditApplicantType { get; set; }
        public EditAdministratorViewModel EditAdministrator { get; set; }
        public EditLoanDetailViewModel EditLoanDetail { get; set; }
        public EditApplicantViewModel EditApplicant { get; set; }
        public EditLoanViewModel EditLoan{ get; set; }
        public AddLoanViewModel AddLoan { get; set; }



        public ICommand AddStatusCommand { get { return new RelayCommand(GoStatusCommand); } }
        public ICommand AddMaterialCommand { get { return new RelayCommand(GoMaterialCommand); } }
        public ICommand AddInternCommand { get { return new RelayCommand(GoInternCommand); } }
        public ICommand AddOwnerCommand { get { return new RelayCommand(GoOwnerCommand); } }
        public ICommand AddMaterialTypeCommand { get { return new RelayCommand(GoMaterialTypeCommand); } }
        public ICommand AddAdministratorCommand { get { return new RelayCommand(GoAdministratorCommand); } }
        public ICommand AddApplicantTypeCommand { get { return new RelayCommand(GoApplicantTypeCommand); } }
        public ICommand AddLoanDetailCommand { get { return new RelayCommand(GoLoanDetailCommand); } }
        public ICommand AddApplicantCommand { get { return new RelayCommand(GoApplicantCommand); } }
        public ICommand AddLoanCommand { get { return new RelayCommand(GoLoanCommand); } }
        private async void GoStatusCommand()
        {
            this.AddStatus = new AddStatusViewModel();
            await App.Navigator.PushAsync(new AddStatusPage());
        }
        private async void GoMaterialCommand()
        {
            this.AddMaterial = new AddMaterialViewModel();
            await App.Navigator.PushAsync(new AddMaterialPage());
        }
        private async void GoInternCommand()
        {
            this.AddIntern = new AddInternViewModel();
            await App.Navigator.PushAsync(new AddInternPage());
        }
        private async void GoOwnerCommand()
        {
            this.AddOwner = new AddOwnerViewModel();
            await App.Navigator.PushAsync(new AddOwnerPage());
        }
        private async void GoMaterialTypeCommand()
        {
            this.AddMaterialType = new AddMaterialTypeViewModel();
            await App.Navigator.PushAsync(new AddMaterialTypePage());
        }
        private async void GoAdministratorCommand()
        {
            this.AddAdministrator = new AddAdministratorViewModel();
            await App.Navigator.PushAsync(new AddAdministratorPage());
        }

        private async void GoApplicantTypeCommand()
        {
            this.AddApplicantType = new AddApplicantTypeViewModel();
            await App.Navigator.PushAsync(new AddApplicantTypePage());
        }

        private async void GoLoanDetailCommand()
        {
            this.AddLoanDetail = new AddLoanDetailViewModel();
            await App.Navigator.PushAsync(new AddLoanDetailPage());
        }
        private async void GoApplicantCommand()
        {
            this.AddApplicant = new AddApplicantViewModel();
            await App.Navigator.PushAsync(new AddApplicantPage());
        }
        private async void GoLoanCommand()
        {
            this.AddLoan = new AddLoanViewModel();
            await App.Navigator.PushAsync(new AddLoanPage());
        }

        public MainViewModel()
        {
            instance = this;
            //this.Login = new LoginViewModel();
            LoadMenu();
        }

        private void LoadMenu()
        {
            var menus = new List<Menu>
            {
                new Menu
                {
                    Icon = "admin",
                    PageName = "AdministratorsPage",
                    Title = "Administrators"
                },
                new Menu
                {
                    Icon = "applicant",
                    PageName = "ApplicantPage",
                    Title = "Applicants"
                },
                new Menu
                {
                    Icon = "applicantType",
                    PageName = "ApplicantTypesPage",
                    Title = "Applicant Types"
                },
                new Menu
                {
                    Icon = "internt",
                    PageName = "InternsPage",
                    Title = "Interns"
                },
                new Menu
                {
                    Icon = "loans",
                    PageName = "LoansPage",
                    Title = "Loans"
                },
                new Menu
                {
                    Icon = "loansDetails",
                    PageName = "LoanDetailsPage",
                    Title = "Loan Details"
                },
                new Menu
                {
                    Icon = "material",
                    PageName = "MaterialsPage",
                    Title = "Materials"
                },
                new Menu
                {
                    Icon = "materialType",
                    PageName = "MaterialTypesPage",
                    Title = "Material Types"
                },
                new Menu
                {
                    Icon = "owner",
                    PageName = "OwnersPage",
                    Title = "Owners"
                },
                new Menu
                {
                    Icon = "status",
                    PageName = "StatusesPage",
                    Title = "Status"
                },
                new Menu
                {
                    Icon = "setup",
                    PageName = "SetupPage",
                    Title = "Setup"
                },
                new Menu
                {
                    Icon = "info",
                    PageName = "AboutPage",
                    Title = "About"
                },
                new Menu
                {
                    Icon = "exit",
                    PageName = "LoginPage",
                    Title = "Logout"
                },
                new Menu
                {
                    Icon = "Prueba",
                    PageName = "pruebaPage",
                    Title = "prueba"
                },
            };
            this.Menus = new ObservableCollection<MenuItemViewModel>(menus.Select(m => new
            MenuItemViewModel
            {
                Icon = m.Icon,
                PageName = m.PageName,
                Title = m.Title
            }).ToList());
        }

        public static MainViewModel GetInstance()
        {
            if (instance == null)
            {
                return new MainViewModel();
            }
            return instance;
        }
    }
}
