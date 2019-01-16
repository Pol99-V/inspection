using System;
using System.Collections.Generic;
using System.Text;

namespace project.Model
{
    public class OperationModel
    {
        public string OperationGUIDfld { get; set; }

        public string Operationfld { get; set; }
    }

    public class SiteModel
    {
        public string ShaftGUIDfld { get; set; }
        public string ShaftNamefld { get; set; }
    }

    public class CompanyModel
    {
        public string CompanyGUIDfld { get; set; }
        public string CompanyNamefld { get; set; }
    }

    public class AssetModel
    {
        public string AssetGUIDfld { get; set; }
        public string AssetNamefld { get; set; }
    }
    public class AuditorModel
    {
        public string PersonGUIDfld { get; set; }
        public string PersonLookupNamefld { get; set; }
    }

    public class ResponModel
    {
        public string PersonGUIDfld { get; set; }
        public string PersonLookupNamefld { get; set; }
    }
}
