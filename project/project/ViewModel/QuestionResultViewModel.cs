using project.Model;
using project.View;
using System;
using System.ComponentModel;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;
using Plugin.Media;
using project.Service;
using Plugin.Permissions;
using Plugin.Permissions.Abstractions;

namespace project.ViewModel
{
    class QuestionResultViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged = delegate { };
        public INavigation Navigation { get; set; }
        public ContentPage page;

        public bool is_finished = false;

        public string text_yes { get; set; }
        public string text_no { get; set; }

        public bool is_button { get; set; }
        public bool is_photo { get; set; }

        public string Text_tile { get; set; }
        public string _title
        {
            get
            {
                Questionnaire naire = QuestionDatas._QuestionData[QuestionDatas.x];
                Questions question = naire.questions[QuestionDatas.y];
                return question.ObservedResultfld;
            }
            set
            {
                Questionnaire naire = QuestionDatas._QuestionData[QuestionDatas.x];
                Questions question = naire.questions[QuestionDatas.y];
                question.ObservedResultfld = value;
            }
        }

        public string _remark
        {
            get
            {
                Questionnaire naire = QuestionDatas._QuestionData[QuestionDatas.x];
                Questions question = naire.questions[QuestionDatas.y];
                return question.Remarksfld;
            }
            set
            {
                Questionnaire naire = QuestionDatas._QuestionData[QuestionDatas.x];
                Questions question = naire.questions[QuestionDatas.y];
                question.Remarksfld = value;
            }
        }

        public string m_image { get; set; }

        public ICommand SubmitYes { protected set; get; }
        public ICommand SubmitNo { protected set; get; }

        public void OnSubmitYes()
        {
            Questionnaire naire = QuestionDatas._QuestionData[QuestionDatas.x];
            Questions question = naire.questions[QuestionDatas.y];
            question.ResultYesNofld = "yes";
            UpdateAll();
        }

        public void OnSubmitNo()
        {
            Questionnaire naire = QuestionDatas._QuestionData[QuestionDatas.x];
            Questions question = naire.questions[QuestionDatas.y];
            question.ResultYesNofld = "no";
            UpdateAll();
        }



        public ICommand SubmitPhoto { protected set; get; }


        public async Task OnSubmitPhoto ()
        {
            await CrossMedia.Current.Initialize();

            if (!CrossMedia.Current.IsCameraAvailable || !CrossMedia.Current.IsTakePhotoSupported)
            {
                await page.DisplayAlert("No Camera", ":( No camera avaialble.", "OK");
                return;
            }


            string file_name = QuestionDatas.x.ToString() + QuestionDatas.y.ToString();
            var file = await CrossMedia.Current.TakePhotoAsync(new Plugin.Media.Abstractions.StoreCameraMediaOptions
            {
                PhotoSize = Plugin.Media.Abstractions.PhotoSize.Medium,
                Directory = "Sample",
                Name = "image.jpg" + file_name
            });

            if (file == null)
                return;

            m_image = file.Path;

            Questionnaire naire = QuestionDatas._QuestionData[QuestionDatas.x];
            Questions question = naire.questions[QuestionDatas.y];
            question.Photofld_name = m_image;

            UpdateAll();
            
        }

        public void OnSubmitGoBack()
        {
            is_Selected();
            var _page = new NavigationPage(new QuestionnairePage(false));
            _page.BarBackgroundColor = Color.FromHex("#fe8e00");
            App.Current.MainPage = _page;
        }

        public ICommand SubmitNext { protected set; get; }

        public void is_Selected()
        {
            Questionnaire naire = QuestionDatas._QuestionData[QuestionDatas.x];
            Questions question = naire.questions[QuestionDatas.y];
            if (question.Remarksfld == null) { question.Selected = false; return; }
            if (question.Remarksfld.Length != 0) question.Selected = true;
            else question.Selected = false;
        }

        public async Task OnSubmitNext()
        {

            is_Selected();

            int total_x,total_y;
            int _x, _y;
            _x = QuestionDatas.x;
            _y = QuestionDatas.y;

            //while (true)
            {
                total_x = QuestionDatas._QuestionData.Count;
                total_y = QuestionDatas._QuestionData[_x].questions.Count;

                if (_y + 1 < total_y) _y++;
                else if (_x + 1 < total_x)
                {
                    _x++;
                    _y = 0;
                }
                else
                {
                    if (is_finished == true) return;
                    is_finished = true;
                    await Navigation.PushAsync(new FinalPage());
                }
                //if (QuestionDatas._QuestionData[_x].questions[_y].Selected == true) break;
            }
            QuestionDatas.x = _x;
            QuestionDatas.y = _y;
            UpdateAll();
        }
        public ICommand SubmitPrev { protected set; get; }
        public ICommand SubmitGoBack { protected set; get; }

        public async Task OnSubmitPrev()
        {

            is_Selected();

            int total_x, total_y;
            int _x, _y;

            _x = QuestionDatas.x;
            _y = QuestionDatas.y;
            //while (true)
            {

                total_x = QuestionDatas._QuestionData.Count;
                total_y = QuestionDatas._QuestionData[_x].questions.Count;

                if (_y > 0) _y--;
                else if (_x > 0)
                {
                    _x--;
                    _y = QuestionDatas._QuestionData[_x].questions.Count - 1;
                }
                else
                {
                    if (is_finished == true) return;
                    is_finished = true;
                    await Navigation.PushAsync(new QuestionnairePage(false));
                    Navigation.RemovePage(page);
                }
                //if (QuestionDatas._QuestionData[_x].questions[_y].Selected == true) break;
            }
            QuestionDatas.x = _x;
            QuestionDatas.y = _y;
            UpdateAll();
        }


        public QuestionResultViewModel(INavigation navigation, ContentPage page)
        {

            this.Navigation = navigation;
            this.page = page;
            is_finished = false;

            SubmitNext = new Command(async () => await OnSubmitNext());
            SubmitPrev = new Command(async () => await OnSubmitPrev());

            SubmitYes = new Command(OnSubmitYes);
            SubmitNo = new Command(OnSubmitNo);


            SubmitPhoto = new Command(async () => await OnSubmitPhoto());
            SubmitGoBack = new Command(OnSubmitGoBack);

            UpdateAll();

        }

        public void UpdateAll()
        {
            Questionnaire naire = QuestionDatas._QuestionData[QuestionDatas.x];
            Questions question = naire.questions[QuestionDatas.y];
            Text_tile = question.Item;
            _title = question.ObservedResultfld;
            _remark = question.Remarksfld;
            m_image = question.Photofld_name;

            if(m_image == null)
            {
                is_button = true;
                is_photo = false;
            }
            else
            {
                is_button = false;
                is_photo = true;
            }

            if (question.ResultYesNofld == null) question.ResultYesNofld = "yes";

            if (question.ResultYesNofld.Equals("yes"))
            {
                text_yes = "● Yes";
                text_no = "○ No";
            }
            else
            {
                text_yes = "○ Yes";
                text_no = "● No";
            }

            
        
            PropertyChanged(this, new PropertyChangedEventArgs("Text_tile"));
            PropertyChanged(this, new PropertyChangedEventArgs("_title"));
            PropertyChanged(this, new PropertyChangedEventArgs("_remark"));
            PropertyChanged(this, new PropertyChangedEventArgs("m_image"));
            PropertyChanged(this, new PropertyChangedEventArgs("text_yes"));
            PropertyChanged(this, new PropertyChangedEventArgs("text_no"));
            PropertyChanged(this, new PropertyChangedEventArgs("is_button"));
            PropertyChanged(this, new PropertyChangedEventArgs("is_photo"));
        }
    }
}
