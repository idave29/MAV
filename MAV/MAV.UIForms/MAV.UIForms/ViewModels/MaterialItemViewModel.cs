using GalaSoft.MvvmLight.Command;
using MAV.Common.Models;
using MAV.UIForms.Views;
using System;
using System.Windows.Input;

namespace MAV.UIForms.ViewModels
{
    public class MaterialItemViewModel : MaterialResponse
    {
        public ICommand SelectMaterialCommand { get { return new RelayCommand(SelectMaterial); } }

        private async void SelectMaterial()
        {
            MainViewModel.GetInstance().EditMaterial = new EditMaterialViewModel((MaterialResponse)this);
            await App.Navigator.PushAsync(new EditMaterialPage());
        }
    }
}