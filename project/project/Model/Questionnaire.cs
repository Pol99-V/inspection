using System;
using System.Collections.Generic;
using System.Text;
using System.Collections.ObjectModel;
using System.Collections;

namespace project.Model
{
    public static class QuestionDatas
    {
        public static ObservableCollection<Questionnaire> _QuestionData { get; set; }
        public static int x = 0, y = 0;
        public static DataType inspec_type = new DataType();
        public static DateTime date = DateTime.Today;
        public static OperationModel operation = new OperationModel();
        public static SiteModel site = new SiteModel();
        public static AssetModel asset = new AssetModel();
        public static CompanyModel company = new CompanyModel();
        public static AuditorModel auditor = new AuditorModel();
        public static ResponModel respon = new ResponModel();
        public static string AuditorSignaturefld_final;
        public static string RespPersonSignaturefld_final;
    }
    public class Questions
    {
        public string Item { get; set; }
        public string EvaluationMasterItemGUIDfld { get; set; }
        public string ParentGUIDfld { get; set; }
        public string Sequencefld { get; set; }

        public string ResultYesNofld { get; set; }
        public string ObservedResultfld { get; set; }
        public string Remarksfld {get; set;}
        public string Photofld_name { get; set; }

        public bool Selected { get; set; }

    }
    public class Questionnaire
    {

        public string header { get; set; }
        public int QuestionnaireRowHeightRequest
        {
            get
            {
                if (questions != null)
                {
                    int cos = 0;
                    foreach(Questions question in questions)
                        cos += question.Item.Length / 56;
                    //return (questions.Count + cos) * 31;
                    //return questions.Count * 33;
                    return questions.Count * 30 + cos * 13;
                }
                    
                else
                    return 0;
            }
            set
            {

            }
        }
        public ObservableCollection<Questions> questions { get; set; }
    }

    public class Questionnaire_final
    {

        public string header { get; set; }
        public List<Questions> questions { get; set; }
    }

}
