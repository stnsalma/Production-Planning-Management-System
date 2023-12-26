using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace PPMS.Models
{
    public class Pro_HandsetSmtModel
    {
        public Pro_HandsetSmtModel ()
        {
            HSmtFilesDetails = new List<FilesDetail>();
            HIqcFilesDetails = new List<FilesDetail>();
            HTrialFilesDetails = new List<FilesDetail>();
            HMpFilesDetails = new List<FilesDetail>();
        }
        
        public List<FilesDetail> HSmtFilesDetails { get; set; }
        public List<FilesDetail> HIqcFilesDetails { get; set; }
        public List<FilesDetail> HTrialFilesDetails { get; set; }
        public List<FilesDetail> HMpFilesDetails { get; set; }
       
        public long HandsetSmtId { get; set; }
        public long PlanId { get; set; }
        public long? ProjectMasterID { get; set; }
        public string ProjectName { get; set; }
        public int? OrderNumber { get; set; }
        public string PoCategory { get; set; }
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime? MaterialReceive_SDate_Auto { get; set; }
         [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime? MaterialReceive_EDate_Auto { get; set; }
         [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime? MaterialReceive_SDate_Manual { get; set; }
         [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime? MaterialReceive_EDate_Manual { get; set; }
         [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime? Iqc_SDate_Auto { get; set; }
         [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime? Iqc_EDate_Auto { get; set; }
         [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime? Iqc_SDate_Manual { get; set; }
         [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime? Iqc_EDate_Manual { get; set; }
         [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime? Trial_SDate_Auto { get; set; }
         [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime? Trial_EDate_Auto { get; set; }
         [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
         public DateTime? Trial_SDate_Manual { get; set; }
         [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime? Trial_EDate_Manual { get; set; }
         [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime? MassProduction_SDate_Auto { get; set; }
         [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime? MassProduction_EDate_Auto { get; set; }
         [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime? MassProduction_SDate_Manual { get; set; }
         [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime? MassProduction_EDate_Manual { get; set; }
         [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime? Packing_SDate_Auto { get; set; }
         [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime? Packing_EDate_Auto { get; set; }
         [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime? Packing_SDate_Manual { get; set; }
         [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime? Packing_EDate_Manual { get; set; }
         [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public long? TotalOrderQuantity { get; set; }
        public string HandsetSmtStatus { get; set; }
        public bool? IsActive { get; set; }
        public string MaterialRules { get; set; }
        public long? Added { get; set; }
        public DateTime? AddedDate { get; set; }
        public long? Updated { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public string HandsetSmtDelayReason { get; set; }
        public string HandsetIqcDelayReason { get; set; }
        public string HandsetTrialDelayReason { get; set; }
        public string HandsetMpDelayReason { get; set; }
        public string HandsetPackingDelayReason { get; set; }
        //Attachment
        public string HandsetSmtAttachment { get; set; }
        public string HandsetIqcAttachment { get; set; }
        public string HandsetTrialAttachment { get; set; }
        public string HandsetMpAttachment { get; set; }
    }
}