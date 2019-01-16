using Newtonsoft.Json;
using Plugin.Connectivity;
using project.Model;
using project.Service;
using System;
using System.Collections.ObjectModel;
using System.IO;
using Xamarin.Forms;


namespace project
{
    public class App : Application
    {
        public static bool IsUserLoggedIn { get; set; }
        public static bool is_stored = false;

        public App()
        {

            if (!IsUserLoggedIn)
            {
                var page = new NavigationPage(new project.View.LoginPage());
                page.BarBackgroundColor = Color.FromHex("#fe8e00");
                MainPage = page;
            }
            else
            {
                var page = new NavigationPage(new project.View.HomePage());
                page.BarBackgroundColor = Color.FromHex("#fe8e00");
                MainPage = page;
            }
        }

        protected override void OnStart()
        {
            // Handle when your app starts
        }

        protected override void OnSleep()
        {
            // Handle when your app sleeps


        }

        protected override void OnResume()
        {
            // Handle when your app resumes
            //if(App.is_stored)
            {
                if (CrossConnectivity.Current.IsConnected)
                {

                    string line;
                    string path = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
                    string filePath = Path.Combine(path, "Data.txt");

                    if(File.Exists(filePath))
                    {
                        using (StreamReader reader = new StreamReader(filePath))
                        {
                            line = reader.ReadLine();
                            QuestionDatas._QuestionData = JsonConvert.DeserializeObject<ObservableCollection<Questionnaire>>(line);
                            QuestionDatas.AuditorSignaturefld_final = reader.ReadLine();
                            QuestionDatas.RespPersonSignaturefld_final = reader.ReadLine();
                            MainPage.DisplayAlert("Message", "Your last data published on server.", "OK");
                            MyHttp.RetryPost();

                            File.Delete(filePath);
                            App.is_stored = false;
                            var page = new NavigationPage(new project.View.HomePage());
                            page.BarBackgroundColor = Color.FromHex("#fe8e00");
                            App.Current.MainPage = page;
                        }
                    }
                }
                    
            }
        }
    }
}
