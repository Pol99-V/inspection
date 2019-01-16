
using project.ViewModel;
using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace project.View
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class QuestionResultPage : ContentPage
	{
        QuestionResultViewModel vm;

		public QuestionResultPage ()
		{
            vm = new QuestionResultViewModel(Navigation, this);
            this.BindingContext = vm;
            InitializeComponent();
        }
        protected override void OnAppearing()
        {
            vm.is_finished = false;
        }

        protected override bool OnBackButtonPressed()
        {
            // If you want to continue going back
            base.OnBackButtonPressed();

            //await Navigation.PushAsync(new QuestionnairePage(false));

            vm.is_Selected();
            Device.BeginInvokeOnMainThread(async () =>
            {

                await Navigation.PushAsync(new QuestionnairePage(false));
                Navigation.RemovePage(this);
            });

            return true;

        }


    }
}