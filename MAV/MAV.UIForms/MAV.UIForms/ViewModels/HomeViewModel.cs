using MAV.Common.Models;
using MAV.Common.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xamarin.Forms;
using System.Windows.Input;

namespace MAV.UIForms.ViewModels
{
    public class HomeViewModel : BaseViewModel
    {
        private string greeting;
        public string Greeting
        {
            get { return this.greeting; }
            set { this.SetValue(ref this.greeting, value); }

        }

        private ApiService apiService;

        private void LoadMaterialTypes()
        {
            int dateTime = DateTime.Now.Hour;

            if (dateTime >= 0 && dateTime <= 11)
            {
                Greeting = "Good Morning";
            }
            else if (dateTime >= 12 && dateTime <= 17)
            {
                Greeting = "Good Afternoon";
            }
            else if (dateTime >= 18 && dateTime <= 23)
            {
                Greeting = "Good Night";
            }

        }

        public HomeViewModel()
        {
            this.apiService = new ApiService();
            this.LoadMaterialTypes(); 
        }
    }
}
