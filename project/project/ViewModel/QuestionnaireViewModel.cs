using project.Model;
using project.View;
using System;
using System.ComponentModel;
using System.Threading.Tasks;
using Xamarin.Forms;
using System.Collections.ObjectModel;
using System.Windows.Input;
using project.Service;

namespace project.ViewModel
{

    class QuestionnaireViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged = delegate { };

        public Action DisplayError;
        public Action DisplayCheckError;

        public INavigation Navigation { get; set; }
        public ContentPage page;

        public ICommand SubmitGoBack { protected set; get; }

        public void OnSubmitGoBack()
        {
            var _page = new NavigationPage(new HeaderPage());
            _page.BarBackgroundColor = Color.FromHex("#fe8e00");
            App.Current.MainPage = _page;
        }

        private bool isbusy;
        public bool isBusy
        {
            get
            {
                return isbusy;
            }
            set
            {
                isbusy = value;
            }
        }

        private Questions _seletedData { get; set; }
        public Questions SeletedData
        {
            get { return _seletedData; }
            set
            {
                _seletedData = value;
            }
        }

        private ObservableCollection<Questionnaire> _questionnaire;

        public ObservableCollection<Questionnaire> _Questionnaire
        {
            get
            {
                if (_questionnaire.Count != 0)
                {
                    isBusy = false;
                    PropertyChanged(this, new PropertyChangedEventArgs("isBusy"));
                }
                return _questionnaire;
            }
            set
            {
                _questionnaire = value;
            }
        }


        public QuestionnaireViewModel(INavigation navigation, ContentPage page,bool is_new)
        {

            this.Navigation = navigation;
            this.page = page;
            QuestionDatas.x = 0;
            QuestionDatas.y = -1;
            isBusy = true;
            _Questionnaire = new ObservableCollection<Questionnaire>();

            if (is_new)
            {
                MyHttp.GetAllQuestions(list =>
                {
                    if (list.Count != 0)
                    {
                        if (list[0].header == null)
                        {
                            isBusy = false;
                            PropertyChanged(this, new PropertyChangedEventArgs("isBusy"));
                            DisplayError();
                        }
                    }
                    foreach (Questionnaire item in list)
                    {
                        foreach (Questions que in item.questions)
                        {
                            //int _tempnaire = QuestionDatas._QuestionData.IndexOf(item);
                            //int _tempque = QuestionDatas._QuestionData[_tempnaire].questions.IndexOf(que);
                            //que.Selected = QuestionDatas._QuestionData[_tempnaire].questions[_tempque].Selected;
                            que.Selected = false;
                        }
                        _Questionnaire.Add(item);
                    }

                });
                QuestionDatas._QuestionData = _Questionnaire;
            }
            else
            {
                MyHttp.GetAllQuestions(list =>
                {
                    if (list.Count != 0)
                    {
                        if (list[0].header == null)
                        {
                            isBusy = false;
                            PropertyChanged(this, new PropertyChangedEventArgs("isBusy"));
                            DisplayError();
                            Navigation.RemovePage(page);
                        }
                    }
                    foreach (Questionnaire item in QuestionDatas._QuestionData)
                    {
                        foreach (Questions que in item.questions)
                        {
                            //int _tempnaire = QuestionDatas._QuestionData.IndexOf(item);
                            //int _tempque = QuestionDatas._QuestionData[_tempnaire].questions.IndexOf(que);
                            //que.Selected = QuestionDatas._QuestionData[_tempnaire].questions[_tempque].Selected;
                            //que.Selected = false;
                        }
                        _Questionnaire.Add(item);
                    }

                });


            }

            SubmitNext = new Command(async () => await OnSubmitNext());

            SubmitGoBack = new Command(OnSubmitGoBack);
        }

        public ICommand SubmitNext { protected set; get; }
        public ICommand RefreshCommand { protected set; get; }
        public async Task OnSubmitNext()
        {


            bool check = test();
            if(check == false)
            {
                DisplayCheckError();
                return;
            }

            await Navigation.PushAsync(new FinalPage());
            //Navigation.RemovePage(page);
        }

        public bool test()
        {
            foreach (Questionnaire item in QuestionDatas._QuestionData)
            {
                foreach (Questions que in item.questions)
                    if (que.Selected == true) return true;
            }
            return false;
        }

    }
}
