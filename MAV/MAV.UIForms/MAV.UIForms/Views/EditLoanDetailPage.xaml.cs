using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MAV.UIForms.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class EditLoanDetailPage : ContentPage
    {
        public EditLoanDetailPage()
        {
            InitializeComponent();
            if (lbEstado.Text == "Regresado")
            {
                btnCerrar.IsEnabled = false;
                //btnCerrar.BackgroundColor.
                etObserv.IsEnabled = false;
            }

            //imgMaterial.Source = ImageSource.FromFile(imgMaterial.BindingContext as string);
        }
    }
}