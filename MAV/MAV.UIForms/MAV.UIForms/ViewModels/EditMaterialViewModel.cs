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
    public class EditMaterialViewModel : BaseViewModel
    {
        private readonly ApiService apiService;
        public MaterialResponse Material { get; set; }

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
        public ICommand DeleteCommand { get { return new RelayCommand(Delete); } }
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

        private async void Delete()
        {
            var confirm = await Application.Current.MainPage.DisplayAlert("Confirmar", "¿Seguro que quieres borrarlo", "Si", "No");
            if (!confirm)
                return;

            isEnabled = false;
            isRunning = true;

            var url = Application.Current.Resources["URLApi"].ToString();
            var response = await this.apiService.DeleteAsync(url,
                "/api",
                "/Materials",
                Material.Id,
                "bearer",
                MainViewModel.GetInstance().Token.Token);

            if (!response.IsSuccess)
            {
                await Application.Current.MainPage.DisplayAlert("Error", response.Message, "Aceptar");
                return;
            }
            MainViewModel.GetInstance().Materials.DeleteMaterialInList(Material.Id);
            this.isEnabled = true;
            this.isRunning = false;
            await App.Navigator.PopAsync();
        }

        private async void Save()
        {
            if (string.IsNullOrEmpty(this.Material.Name))
            {
                await Application.Current.MainPage.DisplayAlert("Error", "Debes introducir un material", "Aceptar");
                return;
            }
            if (string.IsNullOrEmpty(this.Material.Label))
            {
                await Application.Current.MainPage.DisplayAlert("Error", "Debes introducir una etiqueta", "Aceptar");
                return;
            }
            if (string.IsNullOrEmpty(this.Material.Brand))
            {
                await Application.Current.MainPage.DisplayAlert("Error", "Debes introducir una marca", "Aceptar");
                return;
            }
            if (string.IsNullOrEmpty(this.Material.MaterialModel))
            {
                await Application.Current.MainPage.DisplayAlert("Error", "Debes introducir un modelo", "Aceptar");
                return;
            }
            if (string.IsNullOrEmpty(this.Material.SerialNum))
            {
                await Application.Current.MainPage.DisplayAlert("Error", "Debes introducir un numero de serie", "Aceptar");
                return;
            }
            if (string.IsNullOrEmpty(this.Material.Function))
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
                Id = Material.Id,
                Name = Material.Name,
                Label = Material.Label,
                Brand = Material.Brand,
                Function = Material.Function,
                MaterialModel = Material.MaterialModel,
                SerialNum = Material.SerialNum,
                Status = StatusRequest.Id,
                MaterialType = MaterialTypeRequest.Id,
                Owner = OwnerRequest.Id,
                ImageArray = imageArray
            };


            var url = Application.Current.Resources["URLApi"].ToString();

            ResponseO<object> response;
            response = await this.apiService.PutAsyncO(url,
                "/api",
                "/Materials/",
                material.Id,
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
            var modifyMaterial = (MaterialResponse)materialResp;

            //var modifyMaterial = (MaterialResponse)response.Result;
            MainViewModel.GetInstance().Materials.UpdateMaterialInList(modifyMaterial);
            this.isEnabled = true;
            this.isRunning = false;
            await App.Navigator.PopAsync();
        }
        public EditMaterialViewModel(MaterialResponse material)
        {
            this.apiService = new ApiService();
            this.IsEnabled = true;
            this.LoadMaterialTypes();
            this.LoadOwners();
            this.LoadStatuses();
            this.Material = material;
            this.ImageSource = Material.ImageURL; 
            //this.ImageSource = "noImage"; 
        }
        //private IList<string> statusList;
        //private IList<string> materialTypeList;
        //private IList<string> ownerList;
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
        //private async void LoadStatuses()
        //{
        //    var url = Application.Current.Resources["URLApi"].ToString();
        //    var response = await this.apiService.GetListAsync<StatusRequest>(
        //        url,
        //        "/api",
        //        "/Status",
        //        "bearer",
        //        MainViewModel.GetInstance().Token.Token);

        //    if (!response.IsSuccess)
        //    {
        //        await Application.Current.MainPage.DisplayAlert("Error", response.Message, "Aceptar");
        //        return;
        //    }
        //    StatusList = ((List<StatusRequest>)response.Result).Select(m => m.Name).ToList();

        //}

        //private async void LoadMaterialTypes()
        //{
        //    var url = Application.Current.Resources["URLApi"].ToString();
        //    var response = await this.apiService.GetListAsync<MaterialTypeRequest>(
        //        url,
        //        "/api",
        //        "/MaterialTypes",
        //        "bearer",
        //        MainViewModel.GetInstance().Token.Token);

        //    if (!response.IsSuccess)
        //    {
        //        await Application.Current.MainPage.DisplayAlert("Error", response.Message, "Aceptar");
        //        return;
        //    }
        //    MaterialTypeList = ((List<MaterialTypeRequest>)response.Result).Select(m => m.Name).ToList();

        //}

        //private async void LoadOwners()
        //{
        //    var url = Application.Current.Resources["URLApi"].ToString();
        //    var response = await this.apiService.GetListAsync<OwnerRequest>(
        //        url,
        //        "/api",
        //        "/Owners",
        //        "bearer",
        //        MainViewModel.GetInstance().Token.Token);

        //    if (!response.IsSuccess)
        //    {
        //        await Application.Current.MainPage.DisplayAlert("Error", response.Message, "Aceptar");
        //        return;
        //    }
        //    OwnerList = ((List<OwnerRequest>)response.Result).Select(m => m.Email).ToList();

        //}
    }
}
