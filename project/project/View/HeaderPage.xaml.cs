
using project.ViewModel;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace project.View
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class HeaderPage : ContentPage
	{
		public HeaderPage ()
		{
            var vm = new HeaderViewModel(Navigation, this);
            vm.DisplayError += () => DisplayAlert("Error", "You can't post data.", "OK");
            vm.DisplayAuditor += () => DisplayAlert("Message", "Please choose one Auditor ", "OK");
            this.BindingContext = vm;
            InitializeComponent ();
		}
	}
}