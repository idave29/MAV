namespace MAV.UIForms.Infrastructure
{
    using MAV.UIForms.ViewModels;
    public class InstanceLocator
    {
        public MainViewModel Main { get; set; }
        public InstanceLocator()
        {
            this.Main = new MainViewModel();
        }
    }
}
