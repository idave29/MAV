namespace MAV.UIForms.ViewModels
{
    using GalaSoft.MvvmLight.Command;
    using MAV.Common.Models;
    using MAV.Common.Services;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Linq;
    using System.Windows.Input;
    using Xamarin.Forms;
    public class EditLoanViewModel : BaseViewModel
    {
        private readonly ApiService apiService;
        public LoanRequest Loan { get; set; }

        private List<LoanDetailsRequest> myLoansD;
        private ObservableCollection<LoanDetailItemViewModel> loansD;
        public ObservableCollection<LoanDetailItemViewModel> LoansD
        {
            get { return this.loansD; }
            set { this.SetValue(ref this.loansD, value); }
        }

        private bool isRunning;
        public bool IsRunning
        {
            get { return isRunning; }
            set { this.SetValue(ref this.isRunning, value); }
        }

        private bool isRefreshing;
        public bool IsRefreshing
        {
            get { return this.isRefreshing; }
            set { this.SetValue(ref this.isRefreshing, value); }
        }

        private bool isEnabled;
        public bool IsEnabled
        {
            get { return isEnabled; }
            set { this.SetValue(ref this.isEnabled, value); }
        }
        public ICommand SaveCommand { get { return new RelayCommand(Save); } }
        public ICommand DeleteCommand { get { return new RelayCommand(Delete); } }

        public void RefreshLoanDList()
        {
            this.LoansD = new ObservableCollection<LoanDetailItemViewModel>(myLoansD.Select(l => new LoanDetailItemViewModel
            {
                Id = l.Id,
                DateTimeIn = l.DateTimeIn,
                DateTimeOut = l.DateTimeOut,
                Material = l.Material,
                Observations = l.Observations,
                Status = l.Status
            }).OrderBy(l => l.Id).ToList());
        }

        private async void LoadLoans()
        {
            this.IsRefreshing = true;
            var url = Application.Current.Resources["URLApi"].ToString();
            var response = await this.apiService.GetListAsync<LoanDetailsRequest>(
                url,
                "/api",
                "/LoanDetails",
                "bearer",
                MainViewModel.GetInstance().Token.Token);
            this.IsRefreshing = false;
            if (!response.IsSuccess)
            {
                await Application.Current.MainPage.DisplayAlert("Error", response.Message, "Aceptar");
                return;
            }
            myLoansD = (List<LoanDetailsRequest>)response.Result;
            RefreshLoanDList();
        }

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
            this.LoadLoans();
            //this.LoadInterns();
            //this.LoadApplicant();
        }
        //private IList<string> internList;

        //public IList<string> InternList
        //{
        //    get { return this.internList; }
        //    set { this.SetValue(ref this.internList, value); }
        //}

        //private async void LoadInterns()
        //{
        //    var url = Application.Current.Resources["URLApi"].ToString();
        //    var response = await this.apiService.GetListAsync<InternRequest>(
        //        url,
        //        "/api",
        //        "/Interns",
        //        "bearer",
        //        MainViewModel.GetInstance().Token.Token);

        //    if (!response.IsSuccess)
        //    {
        //        await Application.Current.MainPage.DisplayAlert("Error", response.Message, "Aceptar");
        //        return;
        //    }
        //    InternList = ((List<InternRequest>)response.Result).Select(l => l.FullName).ToList();

        //}

        //private IList<string> applicantList;

        //public IList<string> ApplicantList
        //{
        //    get { return this.applicantList; }
        //    set { this.SetValue(ref this.applicantList, value); }
        //}

        //private async void LoadApplicant()
        //{
        //    var url = Application.Current.Resources["URLApi"].ToString();
        //    var response = await this.apiService.GetListAsync<ApplicantRequest>(
        //        url,
        //        "/api",
        //        "/Applicants",
        //        "bearer",
        //        MainViewModel.GetInstance().Token.Token);

        //    if (!response.IsSuccess)
        //    {
        //        await Application.Current.MainPage.DisplayAlert("Error", response.Message, "Aceptar");
        //        return;
        //    }
        //    ApplicantList = ((List<ApplicantRequest>)response.Result).Select(l => l.FullName).ToList();

        //}
    }
}
