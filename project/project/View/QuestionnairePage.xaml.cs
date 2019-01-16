using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using project.ViewModel;
using project.Model;
using System.Collections.ObjectModel;

namespace project.View
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class QuestionnairePage : ContentPage
    {
        QuestionnaireViewModel vm;

        public QuestionnairePage (bool is_new)
		{
            vm = new QuestionnaireViewModel(Navigation, this,is_new);
            vm.DisplayError += () => DisplayAlert("Error", "Server sent you no data", "OK");
            vm.DisplayCheckError += () => DisplayAlert("Alert!", "Please select your questions.", "OK");
            this.BindingContext = vm;
            InitializeComponent ();

            
        }
        
        public void MyList_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            Questions que = (Questions)e.Item;
            que.Selected = true;

            int i = 0, j = 0;
            bool flag = false;
            for(i = 0; i < QuestionDatas._QuestionData.Count; i ++)
            {
                for (j = 0; j < QuestionDatas._QuestionData[i].questions.Count; j++)
                    if (que == QuestionDatas._QuestionData[i].questions[j]) { flag = true; break; }
                if (flag) break;
            }

            QuestionDatas.x = i;
            QuestionDatas.y = j;
            if (flag == false) return;

            var _page = new NavigationPage(new QuestionResultPage());
            _page.BarBackgroundColor = Color.FromHex("#fe8e00");
            App.Current.MainPage = _page;
        }

      

    }
}