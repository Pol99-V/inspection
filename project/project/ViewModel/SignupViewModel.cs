using project.Model;
using System;
using System.ComponentModel;
using System.Windows.Input;
using Xamarin.Forms;
using project.Service;
using System.Threading.Tasks;
using project.View;

namespace project.ViewModel
{
    class SignupViewModel : INotifyPropertyChanged
    {
        public Action DisplayInvalidSignupfalsePrompt;
        public Action DisplayInvalidSignuptruePrompt;
        public Action DisplayServerErrorPrompt;
        public event PropertyChangedEventHandler PropertyChanged = delegate { };

        private string username, password, cpassword;

        public INavigation Navigation { get; set; }
        public ContentPage page;

        public string ConfirmPassword
        {
            get { return cpassword; }
            set
            {
                cpassword = value;
                PropertyChanged(this, new PropertyChangedEventArgs("Email"));
            }
        }

        public string Password
        {
            get { return password; }
            set
            {
                password = value;
                PropertyChanged(this, new PropertyChangedEventArgs("Password"));
            }
        }

        public string Username
        {
            get { return username; }
            set
            {
                username = value;
                PropertyChanged(this, new PropertyChangedEventArgs("Username"));
            }
        }

        public ICommand SubmitSignup { protected set; get; }

        public SignupViewModel(INavigation navigation, ContentPage page)
        {

            this.Navigation = navigation;
            this.page = page;

            SubmitSignup = new Command(async () => await OnSubmitSignup());
        }

        public async Task OnSubmitSignup()
        {
            if(!AreDetailsValid()) DisplayInvalidSignupfalsePrompt();
            else
            {
                var result = await MyHttp.RegisterAsync(username, password);
                //var result = true;
                if (result == true)
                {
                    DisplayInvalidSignuptruePrompt();
                    Navigation.RemovePage(page);
                }
                else DisplayServerErrorPrompt();
            }
        }

        bool AreDetailsValid()
        {
            //return (!string.IsNullOrWhiteSpace(user.Username) && !string.IsNullOrWhiteSpace(user.Password) && !string.IsNullOrWhiteSpace(user.Email) && user.Email.Contains("@"));
            if (password == null || username == null) return false;
            return password.Equals(cpassword);
        }
    }
}
