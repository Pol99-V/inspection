using Newtonsoft.Json;
using Plugin.Connectivity;
using project.Model;
using project.Service;
using project.ViewModel;
using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace project.View
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class HomePage : ContentPage
	{
        private HomeViewModel vm;

        public object MainPage { get; private set; }

        public HomePage ()
		{
            vm = new HomeViewModel(Navigation, this);
            this.BindingContext = vm;
            InitializeComponent ();

            listview.ItemSelected += (sender, e) => {
                ((ListView)sender).SelectedItem = null;
            };
        }

        protected override void OnAppearing()
        {
            if (CrossConnectivity.Current.IsConnected)
            {

                string line;
                string path = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
                string filePath = Path.Combine(path, "Data.txt");

                if (File.Exists(filePath))
                {
                    using (StreamReader reader = new StreamReader(filePath))
                    {
                        line = reader.ReadLine();
                        QuestionDatas._QuestionData = JsonConvert.DeserializeObject<ObservableCollection<Questionnaire>>(line);
                        QuestionDatas.AuditorSignaturefld_final = reader.ReadLine();
                        QuestionDatas.RespPersonSignaturefld_final = reader.ReadLine();
                        Application.Current.MainPage.DisplayAlert("Message", "Your last data published on server.", "OK");
                        MyHttp.RetryPost();

                        File.Delete(filePath);
                    }
                }
            }
            
        }


        public void MyList_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            var _page = new NavigationPage(new HeaderPage());
            _page.BarBackgroundColor = Color.FromHex("#fe8e00");
            App.Current.MainPage = _page;
        }
    }
}