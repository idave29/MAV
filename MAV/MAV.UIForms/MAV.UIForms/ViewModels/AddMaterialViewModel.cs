using GalaSoft.MvvmLight.Command;
using MAV.Common.Helpers;
using MAV.Common.Models;
using MAV.Common.Services;
using Newtonsoft.Json;
using Plugin.Media;
using Plugin.Media.Abstractions;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using Xamarin.Forms;

namespace MAV.UIForms.ViewModels
{
    public class AddMaterialViewModel : BaseViewModel
    {
        private readonly ApiService apiService;
        public string Name { get; set; }

        public string Label { set; get; }

        public string Brand { set; get; }

        public string MaterialModel { set; get; }

        public string SerialNum { set; get; }
        public string Function { set; get; }

        //public string Status { get; set; }

        //public string MaterialType { set; get; }

        //public string Owner { get; set; }

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

        private ImageSource imageSource;
        public ImageSource ImageSource
        {
            get => imageSource;
            set => this.SetValue(ref this.imageSource, value);
        }

        private MediaFile file;

        //private IList<string> statusList;
        //private IList<string> materialTypeList;
        //private IList<string> ownerList;

        private ObservableCollection<StatusRequest> statuses;
        private StatusRequest status; 
        private ObservableCollection<MaterialTypeRequest> materialTypes;
        private MaterialTypeRequest materialType;
        private ObservableCollection<OwnerRequest> owners;
        private OwnerRequest owner;

        //public IList<string> StatusList
        //{
        //    get { return this.statusList; }
        //    set { this.SetValue(ref this.statusList, value); }
        //}

        //public IList<string> MaterialTypeList
        //{
        //    get { return this.materialTypeList; }
        //    set { this.SetValue(ref this.materialTypeList, value); }
        //}

        //public IList<string> OwnerList
        //{
        //    get { return this.ownerList; }
        //    set { this.SetValue(ref this.ownerList, value); }
        //}

        public StatusRequest StatusRequest
        {
            get => this.status;
            set => this.SetValue(ref this.status, value);
        }

        public MaterialTypeRequest MaterialTypeRequest
        {
            get => this.materialType;
            set => this.SetValue(ref this.materialType, value);
        }

        public OwnerRequest OwnerRequest
        {
            get => this.owner;
            set => this.SetValue(ref this.owner, value);
        }

        public ObservableCollection<StatusRequest> Statuses
        {
            get => this.statuses;
            set => this.SetValue(ref this.statuses, value);
        }

        public ObservableCollection<MaterialTypeRequest> MaterialTypes
        {
            get => this.materialTypes;
            set => this.SetValue(ref this.materialTypes, value);
        }
        public ObservableCollection<OwnerRequest> Owners
        {
            get => this.owners;
            set => this.SetValue(ref this.owners, value);
        }


        private async void LoadStatuses()
        {
            var url = Application.Current.Resources["URLApi"].ToString();
            var response = await this.apiService.GetListAsync<StatusRequest>(
                url,
                "/api",
                "/Status",
                "bearer",
                MainViewModel.GetInstance().Token.Token);

            if (!response.IsSuccess)
            {
                await Application.Current.MainPage.DisplayAlert("Error", response.Message, "Aceptar");
                return;
            }
            var myStatuses = ((List<StatusRequest>)response.Result);
            this.Statuses = new ObservableCollection<StatusRequest>(myStatuses);

        }

        private async void LoadMaterialTypes()
        {
            var url = Application.Current.Resources["URLApi"].ToString();
            var response = await this.apiService.GetListAsync<MaterialTypeRequest>(
                url,
                "/api",
                "/MaterialTypes",
                "bearer",
                MainViewModel.GetInstance().Token.Token);

            if (!response.IsSuccess)
            {
                await Application.Current.MainPage.DisplayAlert("Error", response.Message, "Aceptar");
                return;
            }
            var myMaterialTypes = ((List<MaterialTypeRequest>)response.Result);
            this.MaterialTypes = new ObservableCollection<MaterialTypeRequest>(myMaterialTypes);

        }

        private async void LoadOwners()
        {
            var url = Application.Current.Resources["URLApi"].ToString();
            var response = await this.apiService.GetListAsync<OwnerRequest>(
                url,
                "/api",
                "/Owners",
                "bearer",
                MainViewModel.GetInstance().Token.Token);

            if (!response.IsSuccess)
            {
                await Application.Current.MainPage.DisplayAlert("Error", response.Message, "Aceptar");
                return;
            }
            var myOwners = ((List<OwnerRequest>)response.Result);
            this.Owners = new ObservableCollection<OwnerRequest>(myOwners);

        }
        public ICommand SaveCommand { get { return new RelayCommand(Save); } }

        public ICommand ChangeImageCommand => new RelayCommand(this.ChangeImage);


        private async void ChangeImage()
        {
            await CrossMedia.Current.Initialize();

            var source = await Application.Current.MainPage.DisplayActionSheet(
                "¿Donde desea tomar la foto?",
                "Cancel",
                null,
                "Desde la galeria",
                "Desde la cámara");

            if (source == "Cancel")
            {
                this.file = null;
                return;
            }

            if (source == "Desde la cámara")
            {
                this.file = await CrossMedia.Current.TakePhotoAsync(
                    new StoreCameraMediaOptions
                    {
                        Directory = "Pictures",
                        Name = "test.jpg",
                        PhotoSize = PhotoSize.Small,
                    }
                );
            }
            else
            {
                this.file = await CrossMedia.Current.PickPhotoAsync();
            }

            if (this.file != null)
            {
                this.ImageSource = ImageSource.FromStream(() =>
                {
                    var stream = file.GetStream();
                    return stream;
                });
            }
        }

        private async void Save()
        {
            if (string.IsNullOrEmpty(Name))
            {
                await Application.Current.MainPage.DisplayAlert("Error", "Debes introducir un material", "Aceptar");
                return;
            }
            if (string.IsNullOrEmpty(Label))
            {
                await Application.Current.MainPage.DisplayAlert("Error", "Debes introducir una etiqueta", "Aceptar");
                return;
            }
            if (string.IsNullOrEmpty(Brand))
            {
                await Application.Current.MainPage.DisplayAlert("Error", "Debes introducir una marca", "Aceptar");
                return;
            }
            if (string.IsNullOrEmpty(MaterialModel))
            {
                await Application.Current.MainPage.DisplayAlert("Error", "Debes introducir un modelo", "Aceptar");
                return;
            }
            if (string.IsNullOrEmpty(SerialNum))
            {
                await Application.Current.MainPage.DisplayAlert("Error", "Debes introducir un numero de serie", "Aceptar");
                return;
            }
            if (string.IsNullOrEmpty(Function))
            {
                await Application.Current.MainPage.DisplayAlert("Error", "Debes introducir su función", "Aceptar");
                return;
            }
            if (this.StatusRequest == null)
            {
                await Application.Current.MainPage.DisplayAlert("Error", "Debes seleccionar un status", "Aceptar");
                return;
            }
            if (this.MaterialTypeRequest == null)
            {
                await Application.Current.MainPage.DisplayAlert("Error", "Debes seleccionar un tipo de material", "Aceptar");
                return;
            }
            if (this.OwnerRequest == null)
            {
                await Application.Current.MainPage.DisplayAlert("Error", "Debes seleccionar un dueño", "Aceptar");
                return;
            }

            isEnabled = false;
            isRunning = true;

            byte[] imageArray = null;
            if (this.file != null)
            {
                imageArray = FilesHelper.ReadFully(this.file.GetStream());
            }

            var material = new MaterialRequest 
            {
                //Name = "VGA2",
                //Label = "MAV04",
                //Brand = "AMAZON1", 
                //Function = "Manda info chida",
                //MaterialModel = "rojito", 
                //SerialNum = "123456", 
                //Status = 1,
                //MaterialType = 1, 
                //Owner = 1,
                //ImageArray = imageArray

                Name = Name,
                Label = Label,
                Brand = Brand,
                Function = Function,
                MaterialModel = MaterialModel,
                SerialNum = SerialNum,
                Status = StatusRequest.Id,
                MaterialType = MaterialTypeRequest.Id,
                Owner = OwnerRequest.Id,
                ImageArray = imageArray
            };

            var url = Application.Current.Resources["URLApi"].ToString();
            ResponseO<object> response;
            response = await this.apiService.PostAsyncO(url,
                "/api",
                "/Materials/",
                material,
                "bearer",
                MainViewModel.GetInstance().Token.Token);

            if (!response.IsSuccess)
            {
                await Application.Current.MainPage.DisplayAlert("Error", response.Message, "Aceptar");
                return;
            }

            var materialResp = JsonConvert.DeserializeObject<MaterialResponse>(response.Message);
            //var newMaterial = (MaterialResponse)response.Result;
            var newMaterial = (MaterialResponse)materialResp;
            MainViewModel.GetInstance().Materials.AddMaterialToList(newMaterial);
            isEnabled = true;
            isRunning = false;
            await App.Navigator.PopAsync();
        }

        public AddMaterialViewModel()
        {
            this.apiService = new ApiService();
            isEnabled = true;
            this.LoadMaterialTypes();
            this.LoadOwners();
            this.LoadStatuses();
            //this.ImageSource = "noImage"; 
        }
    }
}
