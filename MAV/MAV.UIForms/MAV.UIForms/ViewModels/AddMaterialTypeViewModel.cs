namespace MAV.UIForms.ViewModels
{
    using GalaSoft.MvvmLight.Command;
    using MAV.Common.Models;
    using MAV.Common.Services;
    using System.Windows.Input;
    using Xamarin.Forms;
    public class AddMaterialTypeViewModel : BaseViewModel
    {
        private readonly ApiService apiService;
        public string Name { get; set; }

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

        private async void Save()
        {
            if (string.IsNullOrEmpty(Name))
            {
                await Application.Current.MainPage.DisplayAlert("Error", "Debes introducir un tipo de material", "Aceptar");
                return;
            }

            isEnabled = false;
            isRunning = true;
            var materialtype = new MaterialTypeRequest { Name = Name };
            var url = Application.Current.Resources["URLApi"].ToString();
            var response = await this.apiService.PostAsync(url,
                "/api",
                "/MaterialTypes",
                materialtype,
                "bearer",
                MainViewModel.GetInstance().Token.Token);

            if (!response.IsSuccess)
            {
                await Application.Current.MainPage.DisplayAlert("Error", response.Message, "Aceptar");
                return;
            }
            var newType = (MaterialTypeRequest)response.Result;
            MainViewModel.GetInstance().MaterialTypes.MaterialTypes.Add(newType);
            isEnabled = true;
            isRunning = false;
            await App.Navigator.PopAsync();
        }

        public AddMaterialTypeViewModel()
        {
            this.apiService = new ApiService();
            isEnabled = true;
        }
    }
}
