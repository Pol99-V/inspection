
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using project.ViewModel;

namespace project.View
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class FinalPage : ContentPage
	{

		public FinalPage ()
		{
            var vm = new FinaliseViewModel(Navigation, this);
            this.BindingContext = vm;
            vm.DialogNotCon += () => DisplayAlert("Message", "Your network is offline now.All data has been stored in local.", "OK");
            vm.DialogSuccess += () => DisplayAlert("Success", "successfully finished!", "OK");
            vm.DialogError += () => DisplayAlert("Error", "Server has some issues now,please try again.", "OK");
            vm.DisplayError += () => DisplayAlert("Alert!", "Please fill in the blanks.", "OK");
            InitializeComponent();
        }
    }
}