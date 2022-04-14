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

        private string dateApp;
        public string DateApp
        {
            get { return this.dateApp; }
            set { this.SetValue(ref this.dateApp, value); }

        }

        private ApiService apiService;

        private void LoadMessage()
        {
            int dateTime = DateTime.Now.Hour;
            string hora = DateTime.Now.ToShortTimeString();
            string fecha = DateTime.Now.ToLongDateString();
            
            if (dateTime >= 0 && dateTime <= 11)
            {
                Greeting = "Buenos días!";
            }
            else if (dateTime >= 12 && dateTime <= 17)
            {
                Greeting = "Buenas tardes!";
            }
            else if (dateTime >= 18 && dateTime <= 23)
            {
                Greeting = "Buenas noches!";
            }
            DateApp = fecha; 
        }

        public HomeViewModel()
        {
            this.apiService = new ApiService();
            this.LoadMessage(); 
        }
    }
}
