using System;
using System.Collections.Generic;
using System.Text;

using System.Collections.ObjectModel;

namespace project.Model
{
    public class FinalPostModel
    {
        public List<Questionnaire_final> _qDatas { get; set; }
        public string AuditorSignaturefld { get; set; }
        public string RespPersonSignaturefld { get; set; }
    }
}
