namespace MAV.UIForms.ViewModels
{
    using MAV.Common.Models;
    using MAV.Common.Services;
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Linq;
    using Xamarin.Forms;

    public class InternsViewModel: BaseViewModel
    {
        private ApiService apiService;
        private List<InternRequest> myIntern;
        private ObservableCollection<InternItemViewModel> intern;
        public ObservableCollection<InternItemViewModel> Interns
        {
            get { return this.intern; }
            set { this.SetValue(ref this.intern, value); }
        }

        private bool isRefreshing;
        public bool IsRefreshing
        {
            get { return this.isRefreshing; }
            set { this.SetValue(ref this.isRefreshing, value); }
        }
        public InternsViewModel()
        {
            this.apiService = new ApiService();
            this.LoadInterns();
        }

        private async void LoadInterns()
        {
            this.IsRefreshing = true;
            var url = Application.Current.Resources["URLApi"].ToString();
            var response = await this.apiService.GetListAsync<InternRequest>(
                url,
                "/api",
                "/Interns",
                "bearer",
                MainViewModel.GetInstance().Token.Token);
            this.IsRefreshing = false;
            if (!response.IsSuccess)
            {
                await Application.Current.MainPage.DisplayAlert("Error", response.Message, "Aceptar");
                return;
            }
            var myIntern = (List<InternRequest>)response.Result;
            RefreshInternList();
        }
        private void RefreshInternList()
        {
            this.Interns = new ObservableCollection<InternItemViewModel>(myIntern.Select(m => new InternItemViewModel
            {
                Email = m.Email,
                FirstName = m.FirstName,
                Id = m.Id,
                LastName = m.LastName,
                Password = m.Password,
                PhoneNumber = m.PhoneNumber,
            }).OrderBy(m => m.FirstName).ToList());
        }

        public void AddInternToList(InternRequest intern)
        {
            this.myIntern.Add(intern);
            RefreshInternList();
        }

        public void UpdateInternInList(InternRequest intern)
        {
            var previousIntern = myIntern.Where(mt => mt.Id == intern.Id).FirstOrDefault();
            if (previousIntern != null)
            {
                this.myIntern.Remove(previousIntern);
            }
            this.myIntern.Add(intern);
            RefreshInternList();
        }

        public void DeleteInternInList(int internId)
        {
            var previousIntern = myIntern.Where(mt => mt.Id == internId).FirstOrDefault();
            if (previousIntern != null)
            {
                this.myIntern.Remove(previousIntern);
            }
            RefreshInternList();
        }
    }

}
