//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace PPMS.DAL.DbModel
{
    using System;
    using System.Collections.Generic;
    
    public partial class ProjectCategorization
    {
        public long Id { get; set; }
        public string ProjectName { get; set; }
        public string ProductFamily { get; set; }
        public string AssemblyCategory { get; set; }
        public string SmtCategory { get; set; }
        public string HousingCategory { get; set; }
        public Nullable<bool> IsComplete { get; set; }
        public Nullable<long> Added { get; set; }
        public Nullable<System.DateTime> AddedDate { get; set; }
        public Nullable<long> Updated { get; set; }
        public Nullable<System.DateTime> UpdatedDate { get; set; }
    }
}
