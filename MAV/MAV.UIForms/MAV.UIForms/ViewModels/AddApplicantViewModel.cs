using GalaSoft.MvvmLight.Command;
using MAV.Common.Models;
using MAV.Common.Services;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Input;
using Xamarin.Forms;


namespace MAV.UIForms.ViewModels
{
    public class AddApplicantViewModel : BaseViewModel
    {
        private readonly ApiService apiService;
        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Email { get; set; }

        public string PhoneNumber { get; set; }
        public string ApplicantType { get; set; }

        //public ICollection<LoanRequest> Loans { get; set; }

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

        private IList<string> applicantTypeList;
        public IList<string> ApplicantTypeList
        {
            get { return this.applicantTypeList; }
            set { this.SetValue(ref this.applicantTypeList, value); }
        }
        private async void ApplicantTypeLoans()
        {
            var url = Application.Current.Resources["URLApi"].ToString();
            var response = await this.apiService.GetListAsync<ApplicantTypeRequest>(
                url,
                "/api",
                "/ApplicantTypes",
                "bearer",
                MainViewModel.GetInstance().Token.Token);

            if (!response.IsSuccess)
            {
                await Application.Current.MainPage.DisplayAlert("Error", response.Message, "Aceptar");
                return;
            }
            ApplicantTypeList = ((List<ApplicantTypeRequest>)response.Result).Select(m => m.Name.ToString()).ToList();

        }

        //private IList<string> loanList;
        //public IList<string> LoanList
        //{
        //    get { return this.loanList; }
        //    set { this.SetValue(ref this.loanList, value); }
        //}
        //private async void LoadLoans()
        //{
        //    var url = Application.Current.Resources["URLApi"].ToString();
        //    var response = await this.apiService.GetListAsync<LoanRequest>(
        //        url,
        //        "/api",
        //        "/Loans",
        //        "bearer",
        //        MainViewModel.GetInstance().Token.Token);

        //    if (!response.IsSuccess)
        //    {
        //        await Application.Current.MainPage.DisplayAlert("Error", response.Message, "Aceptar");
        //        return;
        //    }
        //    LoanList = ((List<LoanRequest>)response.Result).Select(m => m.Id.ToString()).ToList();

        //}
        public ICommand SaveCommand { get { return new RelayCommand(Save); } }

        private async void Save()
        {
            if (string.IsNullOrEmpty(FirstName))
            {
                await Application.Current.MainPage.DisplayAlert("Error", "Debes introducir un nombre", "Aceptar");
                return;
            }
            if (string.IsNullOrEmpty(LastName))
            {
                await Application.Current.MainPage.DisplayAlert("Error", "Debes introducir un apellido", "Aceptar");
                return;
            }
            if (string.IsNullOrEmpty(Email))
            {
                await Application.Current.MainPage.DisplayAlert("Error", "Debes introducir un email", "Aceptar");
                return;
            }
            if (string.IsNullOrEmpty(PhoneNumber))
            {
                await Application.Current.MainPage.DisplayAlert("Error", "Debes introducir un numero de telefono", "Aceptar");
                return;
            }
            if (string.IsNullOrEmpty(ApplicantType))
            {
                await Application.Current.MainPage.DisplayAlert("Error", "Debes introducir un Applicant Type", "Aceptar");
                return;
            }
            //if (Loans == null)
            //{
            //    await Application.Current.MainPage.DisplayAlert("Error", "Debes introducir un loan", "Aceptar");
            //    return;
            //}

            isEnabled = false;
            isRunning = true;
            var applicant = new ApplicantRequest { FirstName = FirstName, LastName = LastName, Email = Email, PhoneNumber = PhoneNumber, ApplicantType = ApplicantType, Password="123456" };
            var url = Application.Current.Resources["URLApi"].ToString();
            var response = await this.apiService.PostAsync(url,
                "/api",
                "/Applicants",
                applicant,
                "bearer",
                MainViewModel.GetInstance().Token.Token);

            if (!response.IsSuccess)
            {
                await Application.Current.MainPage.DisplayAlert("Error", response.Message, "Aceptar");
                return;
            }
            var newApplicant = (ApplicantRequest)response.Result;
            MainViewModel.GetInstance().Applicants.AddApplicantToList(newApplicant);
            isEnabled = true;
            isRunning = false;
            await App.Navigator.PopAsync();
        }

        public AddApplicantViewModel()
        {
            this.apiService = new ApiService();
            isEnabled = true;
            this.ApplicantTypeLoans();
            //this.LoadLoans();
        }

    }
}
