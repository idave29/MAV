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
    public class EditApplicantViewModel : BaseViewModel
    {
        private readonly ApiService apiService;
        public ApplicantRequest Applicant { get; set; }

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
                "/Applicants",
                Applicant.Id,
                "bearer",
                MainViewModel.GetInstance().Token.Token);

            if (!response.IsSuccess)
            {
                await Application.Current.MainPage.DisplayAlert("Error", response.Message, "Aceptar");
                return;
            }
            MainViewModel.GetInstance().Applicants.DeleteApplicantInList(Applicant.Id);
            this.isEnabled = true;
            this.isRunning = false;
            await App.Navigator.PopAsync();
        }

        private async void Save()
        {
            if (string.IsNullOrEmpty(Applicant.FirstName))
            {
                await Application.Current.MainPage.DisplayAlert("Error", "Debes introducir un nombre", "Aceptar");
                return;
            }
            if (string.IsNullOrEmpty(Applicant.LastName))
            {
                await Application.Current.MainPage.DisplayAlert("Error", "Debes introducir un apellido", "Aceptar");
                return;
            }
            if (string.IsNullOrEmpty(Applicant.Email))
            {
                await Application.Current.MainPage.DisplayAlert("Error", "Debes introducir un email", "Aceptar");
                return;
            }
            if (string.IsNullOrEmpty(Applicant.PhoneNumber))
            {
                await Application.Current.MainPage.DisplayAlert("Error", "Debes introducir un numero de telefono", "Aceptar");
                return;
            }
            if (string.IsNullOrEmpty(Applicant.ApplicantType))
            {
                await Application.Current.MainPage.DisplayAlert("Error", "Debes introducir un Applicant Type", "Aceptar");
                return;
            }

            isEnabled = false;
            isRunning = true;
            var url = Application.Current.Resources["URLApi"].ToString();
            var response = await this.apiService.PutAsync(url,
                "/api",
                "/Applicants",
                Applicant.Id,
                Applicant,
                "bearer",
                MainViewModel.GetInstance().Token.Token);

            if (!response.IsSuccess)
            {
                await Application.Current.MainPage.DisplayAlert("Error", response.Message, "Aceptar");
                return;
            }
            var modifyApplicant = (ApplicantRequest)response.Result;
            MainViewModel.GetInstance().Applicants.UpdateApplicantInList(modifyApplicant);
            this.isEnabled = true;
            this.isRunning = false;
            await App.Navigator.PopAsync();
        }
        public EditApplicantViewModel(ApplicantRequest applicant)
        {
            this.Applicant = applicant;
            this.apiService = new ApiService();
            this.IsEnabled = true;
            this.LoadApplicantTypes();
        }

        private ObservableCollection<ApplicantTypeRequest> applicantTypes;
        private ApplicantTypeRequest applicantType;

        public ApplicantTypeRequest ApplicantTypeRequest
        {
            get => this.applicantType;
            set => this.SetValue(ref this.applicantType, value);
        }

        public ObservableCollection<ApplicantTypeRequest> ApplicantTypes
        {
            get => this.applicantTypes;
            set => this.SetValue(ref this.applicantTypes, value);
        }


        private async void LoadApplicantTypes()
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
            var myStatuses = ((List<ApplicantTypeRequest>)response.Result);
            this.ApplicantTypes = new ObservableCollection<ApplicantTypeRequest>(myStatuses);
            if (!string.IsNullOrEmpty(Applicant.ApplicantType))
            {
                ApplicantTypeRequest = ApplicantTypes.FirstOrDefault(pt => pt.Name == Applicant.ApplicantType);
            }
        }


    }

}
