using project.Model;
using project.View;
using System;
using System.ComponentModel;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;
using Plugin.Media;
using project.Service;
using Plugin.Connectivity;

namespace project.ViewModel
{
    class FinaliseViewModel : INotifyPropertyChanged
    {
        public Action DialogNotCon;
        public Action DialogSuccess;
        public Action DialogError;
        public Action DisplayError;
        public event PropertyChangedEventHandler PropertyChanged = delegate { };
        public INavigation Navigation { get; set; }
        public ContentPage page;

        private bool isbusy;
        public bool isBusy
        {
            get
            {
                return isbusy;
            }
            set
            {
                isbusy = value;
            }
        }

        public string SignatureAudior { get; set; }
        public string SignatureResponsible { get; set; }

        public ICommand SubmitFinalise { protected set; get; }

        public async Task OnSubmitFinalise()
        {

            if (SignatureAudior == null || SignatureResponsible == null)
            {
                DisplayError();
                return;
            }

            if (isBusy == true) return;
            QuestionDatas.AuditorSignaturefld_final = SignatureAudior;
            QuestionDatas.RespPersonSignaturefld_final = SignatureResponsible;

            isBusy = true;
            PropertyChanged(this, new PropertyChangedEventArgs("isBusy"));

            
            if (!CrossConnectivity.Current.IsConnected)
            {
                DialogNotCon();
                var result = await MyHttp.SecondPost(0);
                if (result == true)
                {
                    isBusy = false;
                    PropertyChanged(this, new PropertyChangedEventArgs("isBusy"));
                }
            }
            else
            {
                var result = await MyHttp.SecondPost(1);
                isBusy = false;
                PropertyChanged(this, new PropertyChangedEventArgs("isBusy"));
                if (result == true)
                {
                    DialogSuccess();
                    var page = new NavigationPage(new HomePage());
                    page.BarBackgroundColor = Color.FromHex("#fe8e00");
                    App.Current.MainPage = page;
                }
                else DialogError();
            }
            
        }

        public FinaliseViewModel(INavigation navigation, ContentPage page)
        {
            this.Navigation = navigation;
            this.page = page;
            isBusy = false;

            SubmitFinalise = new Command(async () => await OnSubmitFinalise());
        }

    }
}
