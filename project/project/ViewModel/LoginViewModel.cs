using project.Model;
using project.View;
using System;
using System.ComponentModel;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;
using project.Service;
using System.Net.Http;

namespace project.ViewModel
{
    public class LoginViewModel : INotifyPropertyChanged
    {
        public Action DisplayInvalidLoginPrompt;
        public Action DisplayEmptyLoginPrompt;
        public event PropertyChangedEventHandler PropertyChanged = delegate { };
        public INavigation Navigation { get; set; }
        public ContentPage page;

        public User m_user = new User();
        public string Username
        {
            get { return m_user.Username; }
            set
            {
                m_user.Username = value;
                PropertyChanged(this, new PropertyChangedEventArgs("Username"));
            }
        }
        public string Password
        {
            get { return m_user.Password; }
            set
            {
                m_user.Password = value;
                PropertyChanged(this, new PropertyChangedEventArgs("Password"));
            }
        }
        public ICommand SubmitLogin { protected set; get; }
        
        public ICommand SubmitSignup { protected set; get; }

        public LoginViewModel(INavigation navigation,ContentPage page)
        {

            this.Navigation = navigation;
            this.page = page;

            SubmitLogin = new Command(async () => await OnSubmitLogin());
            SubmitSignup = new Command(async () => await OnSubmitSignup());
        }
        public async Task OnSubmitLogin()
        {
            if(m_user.Username == null || m_user.Password == null)
            {
                DisplayEmptyLoginPrompt();
            }
            else
            {
                var result = await MyHttp.LoginAsync(m_user.Username, m_user.Password);
                //var result = true;
                if (result == false)
                {
                    DisplayInvalidLoginPrompt();
                }
                else
                {
                    App.IsUserLoggedIn = true;

                    var page = new NavigationPage(new HomePage());
                    page.BarBackgroundColor = Color.FromHex("#fe8e00");
                    App.Current.MainPage = page;

                }
            }
     
        }
        public async Task OnSubmitSignup()
        {
            await Navigation.PushAsync(new SignupPage());
        }
    }
}
