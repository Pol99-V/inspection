using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using project.ViewModel;

namespace project.View
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class LoginPage : ContentPage
	{
        public LoginPage()
        {
            var vm = new LoginViewModel(Navigation,this);
            this.BindingContext = vm;
            vm.DisplayInvalidLoginPrompt += () => DisplayAlert("Error", "Invalid Login, try again", "OK");
            vm.DisplayEmptyLoginPrompt += () => DisplayAlert("Error", "Username or Password is empty now. Please enter username and password", "OK");
            InitializeComponent();
        }
    }
}