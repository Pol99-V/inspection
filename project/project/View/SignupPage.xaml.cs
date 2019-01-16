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
	public partial class SignupPage : ContentPage
	{
		public SignupPage ()
		{
            var vm = new SignupViewModel(Navigation, this);
            this.BindingContext = vm;
            vm.DisplayInvalidSignupfalsePrompt += () => DisplayAlert("Retry", "Please enter correct username and password", "OK");
            vm.DisplayInvalidSignuptruePrompt += () => DisplayAlert("Success", "You can login using this username and password", "OK");
            vm.DisplayServerErrorPrompt += () => DisplayAlert("Error", "Server has some issues", "OK");
            InitializeComponent ();

		}
	}
}