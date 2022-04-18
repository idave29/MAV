using GalaSoft.MvvmLight.Command;
using MAV.Common.Models;
using MAV.Common.Services;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Input;
using Xamarin.Forms;

namespace MAV.UIForms.ViewModels
{
    public class AddLoanViewModel: BaseViewModel
    {
        private readonly ApiService apiService;
        //public string Intern { get; set; }
        public string Applicant { get; set; }

        public MaterialRequest Material { get; set; }

        private IList<string> materialList;
        public IList<string> MaterialList
        {
            get { return this.materialList; }
            set { this.SetValue(ref this.materialList, value); }
        }

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

        //private IList<string> internList;

        //public IList<string> InternList
        //{
        //    get { return this.internList; }
        //    set { this.SetValue(ref this.internList, value); }
        //}
        private IList<string> applicantList;

        public IList<string> ApplicantList
        {
            get { return this.applicantList; }
            set { this.SetValue(ref this.applicantList, value); }
        }

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
            ApplicantList = ((List<ApplicantRequest>)response.Result).Where(l => l.Deleted == false).Where(l => l.Debtor == false).Select(l => l.FullName).ToList();
        }

        public ICommand SaveCommand { get { return new RelayCommand(Save); } }

        private async void Save()
        {
            if (string.IsNullOrEmpty(Applicant))
            {
                await Application.Current.MainPage.DisplayAlert("Error", "Debes introducir un Solicitante", "Aceptar");
                return;
            }
            if (string.IsNullOrEmpty(Material.Name))
            {
                await Application.Current.MainPage.DisplayAlert("Error", "Debes introducir un Material", "Aceptar");
                return;
            }

            isEnabled = false;
            isRunning = true;

            var loanDetail = new LoanDetailsRequest { Material = Material };
            var Loan = new LoanRequest{ Applicant = Applicant, LoanDetails = (ICollection<LoanDetailsRequest>)loanDetail };

            var url = Application.Current.Resources["URLApi"].ToString();
            var response = await this.apiService.PostAsync(url,
                "/api",
                "/Loans",
                Loan,
                "bearer",
                MainViewModel.GetInstance().Token.Token);

            if (!response.IsSuccess)
            {
                await Application.Current.MainPage.DisplayAlert("Error", response.Message, "Aceptar");
                return;
            }
            var newLoan = (LoanRequest)response.Result;
            MainViewModel.GetInstance().Loans.AddLoanToList(newLoan);
       
            isEnabled = true;
            isRunning = false;
            await App.Navigator.PopAsync();
        }

        private async void LoadMaterial()
        {
            var url = Application.Current.Resources["URLApi"].ToString();
            var response = await this.apiService.GetListAsync<MaterialRequest>(
                url,
                "/api",
                "/Materials",
                "bearer",
                MainViewModel.GetInstance().Token.Token);

            if (!response.IsSuccess)
            {
                await Application.Current.MainPage.DisplayAlert("Error", response.Message, "Aceptar");
                return;
            }
            MaterialList = ((List<MaterialRequest>)response.Result).Where(t => t.Status == 1).Select(m => m.Name).ToList();

        }

        public AddLoanViewModel()
        {
            this.apiService = new ApiService();
            isEnabled = true;
            //this.LoadInterns();
            this.LoadApplicant();
            this.LoadMaterial();
        }
    }
}
