using GalaSoft.MvvmLight.Command;
using MAV.Common.Models;
using MAV.UIForms.Views;
using System;
using System.Windows.Input;

namespace MAV.UIForms.ViewModels
{
    public class MaterialItemViewModel : MaterialRequest
    {
        public ICommand SelectMaterialCommand { get { return new RelayCommand(SelectMaterial); } }

        private async void SelectMaterial()
        {
            MainViewModel.GetInstance().EditMaterial = new EditMaterialViewModel((MaterialRequest)this);
            await App.Navigator.PushAsync(new EditMaterialPage());
        }
    }
}