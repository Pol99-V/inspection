using project.Model;
using project.View;
using System;
using System.ComponentModel;
using System.Threading.Tasks;
using Xamarin.Forms;
using System.Collections.Generic;
using System.Windows.Input;
using System.Collections.ObjectModel;
using Newtonsoft.Json;
using project.Service;

namespace project.ViewModel
{
    class HeaderViewModel : INotifyPropertyChanged
    {
        public Action DisplayError;
        public Action DisplayAuditor;
        public event PropertyChangedEventHandler PropertyChanged = delegate { };

        public ICommand SubmitNext { protected set; get; }
        public INavigation Navigation { get; set; }
        public ContentPage page;
        
        private DateTime _date;
        public DateTime _Date
        {
            get { return _date; }
            set
            {
                _date = value;
            }
        }

        public ICommand SubmitGoBack{ protected set; get; }

        public void OnSubmitGoBack()
        {
            var _page = new NavigationPage(new HomePage());
            _page.BarBackgroundColor = Color.FromHex("#fe8e00");
            App.Current.MainPage = _page;
        }


        public ObservableCollection<OperationModel> Operations { get; set; }
        public ObservableCollection<SiteModel> Sites { get; set; }
        public ObservableCollection<AssetModel> Assets { get; set; }
        public ObservableCollection<CompanyModel> Companies { get; set; }
        public ObservableCollection<AuditorModel> Auditors { get; set; }
        public ObservableCollection<ResponModel> Respons { get; set; }


        public ObservableCollection<string> operations { get; set; }
        public ObservableCollection<string> sites { get; set; }
        public ObservableCollection<string> companies { get; set; }
        public ObservableCollection<string> assets { get; set; }
        public ObservableCollection<string> respons { get; set; }
        public ObservableCollection<string> auditors { get; set; }

        int _seletedoper,_seletedsite,_seletedasset,_seletedcompany,_seletedauditor = -1,_seletedrespon;
        public int SeletedOper
        {
            get { return _seletedoper; }
            set
            {
                _seletedoper = value;
                QuestionDatas.operation = Operations[_seletedoper];
            }
        }
        public int SeletedSite
        {
            get { return _seletedsite; }
            set
            {
                _seletedsite = value;
                QuestionDatas.site = Sites[_seletedsite];
            }
        }

        public int SeletedAsset
        {
            get { return _seletedasset; }
            set
            {
                _seletedasset = value;
                QuestionDatas.asset = Assets[_seletedasset];
            }
        }

        public int SeletedCompany
        {
            get { return _seletedcompany; }
            set
            {
                _seletedcompany = value;
                QuestionDatas.company = Companies[_seletedcompany];
            }
        }

        public int SeletedRespon
        {
            get { return _seletedrespon; }
            set
            {
                {
                    _seletedrespon = value;
                    QuestionDatas.respon = Respons[_seletedrespon];
                }
            }
        }

        public int SeletedAuditor
        {
            get { return _seletedauditor; }
            set
            {
                {
                    _seletedauditor = value;
                    QuestionDatas.auditor = Auditors[_seletedauditor];
                }
            }
        }

        public HeaderViewModel(INavigation navigation, ContentPage page)
        {

            this.Navigation = navigation;
            this.page = page;
            
            Operations = new ObservableCollection<OperationModel>();
            operations = new ObservableCollection<string>();
            Sites = new ObservableCollection<SiteModel>();
            sites = new ObservableCollection<string>();
            Assets = new ObservableCollection<AssetModel>();
            assets = new ObservableCollection<string>();
            Companies = new ObservableCollection<CompanyModel>();
            companies = new ObservableCollection<string>();
            Auditors = new ObservableCollection<AuditorModel>();
            auditors = new ObservableCollection<string>();
            Respons = new ObservableCollection<ResponModel>();
            respons = new ObservableCollection<string>();



            _Date = DateTime.Today;
            MyHttp.GetAllOperations(list =>
            {
                foreach (OperationModel item in list)
                {
                    Operations.Add(item);
                    operations.Add(item.Operationfld);
                }
            });
            
            MyHttp.GetAllSites(list =>
            {
                foreach (SiteModel item in list)
                {
                    Sites.Add(item);
                    sites.Add(item.ShaftNamefld);
                }
            });

            MyHttp.GetAllAssets(list =>
            {
                foreach (AssetModel item in list)
                {
                    Assets.Add(item);
                    assets.Add(item.AssetNamefld);
                }
            });

            MyHttp.GetAllCompany(list =>
            {
                foreach (CompanyModel item in list)
                {
                    Companies.Add(item);
                    companies.Add(item.CompanyNamefld);
                }
            });

            MyHttp.GetAllAuditors(list =>
            {
                foreach (AuditorModel item in list)
                {
                    Auditors.Add(item);
                    auditors.Add(item.PersonLookupNamefld);
                }
            });

            MyHttp.GetAllRespon(list =>
            {
                foreach (ResponModel item in list)
                {
                    Respons.Add(item);
                    respons.Add(item.PersonLookupNamefld);
                }
            });

    
            SubmitNext = new Command(async () => await OnSubmitNext());
            SubmitGoBack = new Command(OnSubmitGoBack);


        }

        public async Task OnSubmitNext()
        {
            if(_seletedauditor == -1)
            {
                DisplayAuditor();
                return;
            }
            QuestionDatas.date = _date;
            var result = await MyHttp.FirstPost();
            if(result == true)
            {
                var _page = new NavigationPage(new QuestionnairePage(true));
                _page.BarBackgroundColor = Color.FromHex("#fe8e00");
                App.Current.MainPage = _page;
            }
            else DisplayError();
        }
    }
}
