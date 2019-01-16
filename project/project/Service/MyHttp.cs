using project.Model;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Net.Http;
using Newtonsoft.Json;
using System.IO;
using System.Text;

namespace project.Service
{
    public class MyHttp
    {
        private static string basic_url = "https://xamarinbackend20181226112307.azurewebsites.net/api/";
        private static string basic_url_users = basic_url + "users/";
        public static async void GetAllNewsAsync(Action<List<DataType>> action)
        {

            HttpClient httpClient = new HttpClient();
            HttpResponseMessage response = await httpClient.GetAsync(basic_url+ "operation/insp");

            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                var list = JsonConvert.DeserializeObject<List<DataType>>(await response.Content.ReadAsStringAsync());
                action(list);
            }
        }

        public static async void GetAllOperations(Action<List<OperationModel>> action)
        {

            HttpClient httpClient = new HttpClient();
            HttpResponseMessage response = await httpClient.GetAsync(basic_url + "operation");

            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                var list = JsonConvert.DeserializeObject<List<OperationModel>>(await response.Content.ReadAsStringAsync());
                action(list);
            }
        }

        public static async void GetAllSites(Action<List<SiteModel>> action)
        {

            HttpClient httpClient = new HttpClient();
            HttpResponseMessage response = await httpClient.GetAsync(basic_url + "operation/site");

            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                var list = JsonConvert.DeserializeObject<List<SiteModel>>(await response.Content.ReadAsStringAsync());
                action(list);
            }
        }

        public static async void GetAllAssets(Action<List<AssetModel>> action)
        {

            HttpClient httpClient = new HttpClient();
            HttpResponseMessage response = await httpClient.GetAsync(basic_url + "operation/asset");

            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                var list = JsonConvert.DeserializeObject<List<AssetModel>>(await response.Content.ReadAsStringAsync());
                action(list);
            }
        }


        public static async void GetAllCompany(Action<List<CompanyModel>> action)
        {

            HttpClient httpClient = new HttpClient();
            HttpResponseMessage response = await httpClient.GetAsync(basic_url + "operation/company");

            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                var list = JsonConvert.DeserializeObject<List<CompanyModel>>(await response.Content.ReadAsStringAsync());
                action(list);
            }
        }

        public static async void GetAllAuditors(Action<List<AuditorModel>> action)
        {

            HttpClient httpClient = new HttpClient();
            HttpResponseMessage response = await httpClient.GetAsync(basic_url + "operation/auditor");

            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                var list = JsonConvert.DeserializeObject<List<AuditorModel>>(await response.Content.ReadAsStringAsync());
                action(list);
            }
        }

        public static async void GetAllRespon(Action<List<ResponModel>> action)
        {

            HttpClient httpClient = new HttpClient();
            HttpResponseMessage response = await httpClient.GetAsync(basic_url + "operation/respon");

            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                var list = JsonConvert.DeserializeObject<List<ResponModel>>(await response.Content.ReadAsStringAsync());
                action(list);
            }
        }

        public static async void GetAllQuestions(Action<List<Questionnaire>> action)
        {

            HttpClient httpClient = new HttpClient();
            HttpResponseMessage response = await httpClient.GetAsync(basic_url + "operation/questionnaire");

            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                var list = JsonConvert.DeserializeObject<List<Questionnaire>>(await response.Content.ReadAsStringAsync());
                action(list);
            }
        }

        public static async Task<bool> FirstPost()
        {

            FirstPostModel data = new FirstPostModel
            {
                EvaluationTopicGUIDfld = QuestionDatas.inspec_type.EvaluationTopicGUIDfld,
                DateTimeLastUpdatedfld = QuestionDatas.date,
                OperationGUIDfld = QuestionDatas.operation.OperationGUIDfld,
                ShaftGUIDfld = QuestionDatas.site.ShaftGUIDfld,
                AssetGUIDfld = QuestionDatas.asset.AssetGUIDfld,
                CompanyGUIDfld = QuestionDatas.company.CompanyGUIDfld,
                PersonGUIDfld = QuestionDatas.respon.PersonGUIDfld,
                EvaluatorGUIDfld = QuestionDatas.auditor.PersonGUIDfld
            };

            var client = new HttpClient();
            var content = new StringContent(JsonConvert.SerializeObject(data), Encoding.UTF8, "application/json");
            var response = await client.PostAsync(basic_url + "InspHeader/writeHeader", content);
            var list = await response.Content.ReadAsStringAsync();

            if (list.Equals("true")) return true;
            return false;
        }

        public static async Task<bool> SecondPost(int isCon)
        {


            if (isCon == 0)
            {


                string path = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
                string filePath = Path.Combine(path, "Data.txt");
                string json_save = JsonConvert.SerializeObject(QuestionDatas._QuestionData);
                using (var file = File.Open(filePath, FileMode.Create, FileAccess.Write))
                {
                    using (var strm = new StreamWriter(file))
                    {
                        strm.Write(json_save);
                        strm.Write("\n");
                        strm.Write(QuestionDatas.AuditorSignaturefld_final);
                        strm.Write("\n");
                        strm.Write(QuestionDatas.RespPersonSignaturefld_final);
                    }

                }
                App.is_stored = true;
                return true;
            }

            bool isSuccess = true;
            
            int i,j;
            for(i = 0; i < QuestionDatas._QuestionData.Count; i++)
            {
                Questionnaire naire = QuestionDatas._QuestionData[i];
                for (j = 0; j < naire.questions.Count; j ++)
                {
                    if (naire.questions[j].Selected == false) break;
                    HttpClient client = new HttpClient();
                    var content = new MultipartFormDataContent();

                    Questions question = naire.questions[j];
                    if(question.Photofld_name != null)
                    {
                        var upfilebytes = File.ReadAllBytes(question.Photofld_name);
                        ByteArrayContent baContent = new ByteArrayContent(upfilebytes);
                        content.Add(baContent, "File", Path.GetFileName(question.Photofld_name));
                    }

                    StringContent _Header = new StringContent(naire.header);
                    StringContent _Item = new StringContent(question.Item);
                    StringContent _Sequencefld = new StringContent(question.Sequencefld);
                    String is_right = "1";
                    if (question.ResultYesNofld.Equals("no")) is_right = "0";
                    StringContent _ResultYesNofld = new StringContent(is_right);
                    if (question.ObservedResultfld == null) question.ObservedResultfld = "";
                    StringContent _ObservedResultfld = new StringContent(question.ObservedResultfld);
                    if (question.Remarksfld == null) question.Remarksfld = "";
                    StringContent _Remarksfld = new StringContent(question.Remarksfld);

                    StringContent _ParentGUIDfld = new StringContent(question.ParentGUIDfld);
                    StringContent _EvaluationMasterItemGUIDfld = new StringContent(question.EvaluationMasterItemGUIDfld);


                    content.Add(_Header, "header");
                    content.Add(_Item, "Item");
                    content.Add(_Sequencefld, "Sequencefld");
                    content.Add(_ResultYesNofld, "ResultYesNofld");
                    content.Add(_ObservedResultfld, "ObservedResultfld");
                    content.Add(_Remarksfld, "Remarksfld");
                    content.Add(_ParentGUIDfld, "ParentGUIDfld");
                    content.Add(_EvaluationMasterItemGUIDfld, "EvaluationMasterItemGUIDfld");


                    
                    if (QuestionDatas.AuditorSignaturefld_final == null) QuestionDatas.AuditorSignaturefld_final = "";
                    content.Add(new StringContent(QuestionDatas.AuditorSignaturefld_final), "AuditorSignaturefld");
                    if (QuestionDatas.RespPersonSignaturefld_final == null) QuestionDatas.RespPersonSignaturefld_final = "";
                    content.Add(new StringContent(QuestionDatas.RespPersonSignaturefld_final), "RespPersonSignaturefld");

                    var response = await client.PostAsync(basic_url + "result/upload", content);
                    var list = await response.Content.ReadAsStringAsync();


                    if (!list.Equals("true")) isSuccess = false;

                }
            }

            return isSuccess;
        }


        public static async Task<bool> LoginAsync(string userName, string password)
        {

            User user = new User
            {
                Username = userName,
                Password = password
            };
            try
            {
                var client = new HttpClient();
                var content = new StringContent(JsonConvert.SerializeObject(user), Encoding.UTF8, "application/json");
                var response = await client.PostAsync(basic_url_users + "login", content);
                if (response.StatusCode != System.Net.HttpStatusCode.OK) return false;
                var list = await response.Content.ReadAsStringAsync();

                if (list.Equals("true")) return true;
                return false;
            }
            catch
            {
                return false;
            }
        }



        public static async Task<bool> RegisterAsync(string userName, string password)
        {

            User user = new User
            {
                Username = userName,
                Password = password
            };
            try
            {
                var client = new HttpClient();
                var content = new StringContent(JsonConvert.SerializeObject(user), Encoding.UTF8, "application/json");
                var response = await client.PostAsync(basic_url_users + "register", content);
                if (response.StatusCode != System.Net.HttpStatusCode.OK) return false;
                var list = await response.Content.ReadAsStringAsync();

                if (list.Equals("success")) return true;
                return false;

            }
            catch
            {
                return false;
            }
     
        }



        public static async void RetryPost()
        {
            int i, j;
            for (i = 0; i < QuestionDatas._QuestionData.Count; i++)
            {
                Questionnaire naire = QuestionDatas._QuestionData[i];
                for (j = 0; j < naire.questions.Count; j++)
                {

                    if (naire.questions[j].Selected == false) break;

                    HttpClient client = new HttpClient();
                    var content = new MultipartFormDataContent();

                    Questions question = naire.questions[j];
                    if (question.Photofld_name != null)
                    {
                        var upfilebytes = File.ReadAllBytes(question.Photofld_name);
                        ByteArrayContent baContent = new ByteArrayContent(upfilebytes);
                        content.Add(baContent, "File", Path.GetFileName(question.Photofld_name));
                    }

                    StringContent _Header = new StringContent(naire.header);
                    StringContent _Item = new StringContent(question.Item);
                    StringContent _Sequencefld = new StringContent(question.Sequencefld);
                    String is_right = "1";
                    if (question.ResultYesNofld.Equals("no")) is_right = "0";
                    StringContent _ResultYesNofld = new StringContent(is_right);
                    if (question.ObservedResultfld == null) question.ObservedResultfld = "";
                    StringContent _ObservedResultfld = new StringContent(question.ObservedResultfld);
                    if (question.Remarksfld == null) question.Remarksfld = "";
                    StringContent _Remarksfld = new StringContent(question.Remarksfld);

                    StringContent _ParentGUIDfld = new StringContent(question.ParentGUIDfld);
                    StringContent _EvaluationMasterItemGUIDfld = new StringContent(question.EvaluationMasterItemGUIDfld);


                    content.Add(_Header, "header");
                    content.Add(_Item, "Item");
                    content.Add(_Sequencefld, "Sequencefld");
                    content.Add(_ResultYesNofld, "ResultYesNofld");
                    content.Add(_ObservedResultfld, "ObservedResultfld");
                    content.Add(_Remarksfld, "Remarksfld");
                    content.Add(_ParentGUIDfld, "ParentGUIDfld");
                    content.Add(_EvaluationMasterItemGUIDfld, "EvaluationMasterItemGUIDfld");



                    if (QuestionDatas.AuditorSignaturefld_final == null) QuestionDatas.AuditorSignaturefld_final = "";
                    content.Add(new StringContent(QuestionDatas.AuditorSignaturefld_final), "AuditorSignaturefld");
                    if (QuestionDatas.RespPersonSignaturefld_final == null) QuestionDatas.RespPersonSignaturefld_final = "";
                    content.Add(new StringContent(QuestionDatas.RespPersonSignaturefld_final), "RespPersonSignaturefld");

                    var response = await client.PostAsync(basic_url + "result/upload", content);
                    var list = await response.Content.ReadAsStringAsync();


                }
            }

        }
    }
}
