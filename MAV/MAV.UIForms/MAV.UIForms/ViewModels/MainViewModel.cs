namespace MAV.UIForms.ViewModels
{
    public class MainViewModel
    {
        private static MainViewModel instance;
        public LoginViewModel Login { get; set; }
        //public AdministratorsViewModel Administrators { get; set; }
        public StatusesViewModel Statuses { get; set; }
        public OwnersViewModel Owners { get; set; }

        public MainViewModel()
        {
            instance = this;
            //this.Login = new LoginViewModel();
        }
        public static MainViewModel GetInstance()
        {
            if(instance==null)
            {
                return new MainViewModel();
            }
            return instance;
        }
    }
}
