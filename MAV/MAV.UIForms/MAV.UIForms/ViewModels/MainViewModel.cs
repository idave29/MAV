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
        public ICommand HelloCommand { get { return new RelayCommand(Hello); } }

        private async void Hello()
        {
            await App.Navigator.PushAsync(new AboutPage());
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
                }
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
