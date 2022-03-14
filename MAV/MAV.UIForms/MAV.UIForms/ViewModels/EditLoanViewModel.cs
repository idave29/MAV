namespace MAV.UIForms.ViewModels
{
    using GalaSoft.MvvmLight.Command;
    using MAV.Common.Models;
    using MAV.Common.Services;
    using System.Collections.Generic;
    using System.Linq;
    using System.Windows.Input;
    using Xamarin.Forms;
    public class EditLoanViewModel : BaseViewModel
    {
        private readonly ApiService apiService;
        public LoanRequest Loan { get; set; }

        private bool isRunning;
        public bool IsRunning
        {
            get { return isRunning; }
            set { this.SetValue(ref this.isRunning, value); }
        }

        private bool isEnabled;
        public bool IsEnabled
        {
            get { return isEnabled; }
            set { this.SetValue(ref this.isEnabled, value); }
        }
        public ICommand SaveCommand { get { return new RelayCommand(Save); } }
        public ICommand DeleteCommand { get { return new RelayCommand(Delete); } }

        private async void Delete()
        {
            var confirm = await Application.Current.MainPage.DisplayAlert("Confirmar", "¿Seguro que quieres borrarlo", "Si", "No");
            if (!confirm)
                return;

            isEnabled = false;
            isRunning = true;

            var url = Application.Current.Resources["URLApi"].ToString();
            var response = await
                this.apiService.DeleteAsync(url,
                "/api",
                "/Loans",
                Loan.Id,
                "bearer",
                MainViewModel.GetInstance().Token.Token);

            if (!response.IsSuccess)
            {
                await Application.Current.MainPage.DisplayAlert("Error", response.Message, "Aceptar");
                return;
            }
            MainViewModel.GetInstance().Loans.DeleteLoanInList(Loan.Id);
            this.isEnabled = true;
            this.isRunning = false;
            await App.Navigator.PopAsync();
        }

        private async void Save()
        {
            if (string.IsNullOrEmpty(Loan.Intern))
            {
                await Application.Current.MainPage.DisplayAlert("Error", "Debes introducir un Intern", "Aceptar");
                return;
            }
            if (string.IsNullOrEmpty(Loan.Applicant))
            {
                await Application.Current.MainPage.DisplayAlert("Error", "Debes introducir un apellido", "Aceptar");
                return;
            }

            isEnabled = false;
            isRunning = true;
            var url = Application.Current.Resources["URLApi"].ToString();
            var response = await this.apiService.PutAsync(url,
                "/api",
                "/Loans",
                Loan.Id,
                Loan,
                "bearer",
                MainViewModel.GetInstance().Token.Token);

            if (!response.IsSuccess)
            {
                await Application.Current.MainPage.DisplayAlert("Error", response.Message, "Aceptar");
                return;
            }
            var modifyLoan = (LoanRequest)response.Result;
            MainViewModel.GetInstance().Loans.UpdateLoanInList(modifyLoan);
            this.isEnabled = true;
            this.isRunning = false;
            await App.Navigator.PopAsync();
        }
        public EditLoanViewModel(LoanRequest loan)
        {
            this.Loan = loan;
            this.apiService = new ApiService();
            this.IsEnabled = true;
            this.LoadInterns();
            this.LoadApplicant();
        }
        private IList<string> internList;

        public IList<string> InternList
        {
            get { return this.internList; }
            set { this.SetValue(ref this.internList, value); }
        }

        private async void LoadInterns()
        {
            var url = Application.Current.Resources["URLApi"].ToString();
            var response = await this.apiService.GetListAsync<InternRequest>(
                url,
                "/api",
                "/Interns",
                "bearer",
                MainViewModel.GetInstance().Token.Token);

            if (!response.IsSuccess)
            {
                await Application.Current.MainPage.DisplayAlert("Error", response.Message, "Aceptar");
                return;
            }
            InternList = ((List<InternRequest>)response.Result).Select(l => l.FullName).ToList();

        }

        private IList<string> applicantList;

        public IList<string> ApplicantList
        {
            get { return this.applicantList; }
            set { this.SetValue(ref this.applicantList, value); }
        }

        private async void LoadApplicant()
        {
            var url = Application.Current.Resources["URLApi"].ToString();
            var response = await this.apiService.GetListAsync<ApplicantRequest>(
                url,
                "/api",
                "/Applicants",
                "bearer",
                MainViewModel.GetInstance().Token.Token);

            if (!response.IsSuccess)
            {
                await Application.Current.MainPage.DisplayAlert("Error", response.Message, "Aceptar");
                return;
            }
            ApplicantList = ((List<ApplicantRequest>)response.Result).Select(l => l.FullName).ToList();

        }
    }
}
