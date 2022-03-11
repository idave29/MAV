using GalaSoft.MvvmLight.Command;
using MAV.Common.Models;
using MAV.UIForms.Views;
using System;
using System.Windows.Input;

namespace MAV.UIForms.ViewModels
{
    public class MaterialTypeItemViewModel : MaterialTypeRequest
    {
        public ICommand SelectMaterialTypeCommand { get { return new RelayCommand(SelectMaterialType); } }

        private async void SelectMaterialType()
        {
            MainViewModel.GetInstance().EditMaterialType = new EditMaterialTypeViewModel((MaterialTypeRequest)this);
            await App.Navigator.PushAsync(new EditMaterialTypePage());
        }
    }
}
