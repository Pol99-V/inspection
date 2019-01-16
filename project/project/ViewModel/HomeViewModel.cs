using project.Model;
using project.View;

using System.ComponentModel;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;
using project.Service;

using System.Net.Http;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace project.ViewModel
{
    class HomeViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged = delegate { };
        private ObservableCollection<DataType> items;

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
        public ObservableCollection<DataType> Items
        {
            get
            {
                if(items.Count !=0 )
                {
                    isBusy = false;
                    PropertyChanged(this, new PropertyChangedEventArgs("isBusy"));
                }
                   
                return items;
            }
            set
            {
                items = value;
          
            }
        }
        public DataType SeletedData
        {
            get { return QuestionDatas.inspec_type; }
            set
            {
 
                if(value!= null)
                {
                    QuestionDatas.inspec_type = value;
                }

            }
        }

        public async Task HandleSelectedItem()
        {
            var _page = new NavigationPage(new HeaderPage());
            _page.BarBackgroundColor = Color.FromHex("#fe8e00");
            App.Current.MainPage = _page;
        }
        public INavigation Navigation { get; set; }
        public ContentPage page;

        public ICommand SubmitLogout { protected set; get; }
        public ICommand SubmitItem { protected set; get; }

        public HomeViewModel(INavigation navigation, ContentPage page)
        {

            this.Navigation = navigation;
            this.page = page;


            Items = new ObservableCollection<DataType>();
            isBusy = true;
            MyHttp.GetAllNewsAsync(list =>
            {
                int index = 1;
                foreach (DataType item in list)
                {
                    item.EvaluationTopicNamefld = index.ToString() + ". " + item.EvaluationTopicNamefld;
                    index++;
                    Items.Add(item);
                }
            });
            SubmitLogout = new Command(OnSubmitLogout);
            //SubmitItem = new Command(async () => await HandleSelectedItem());
        }

        public void OnSubmitLogout()
        {
            App.IsUserLoggedIn = false;
            var page = new NavigationPage(new LoginPage());
            page.BarBackgroundColor = Color.FromHex("#fe8e00");
            App.Current.MainPage = page;
        }



    }
}
