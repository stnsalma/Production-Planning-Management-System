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
    
    public partial class ProPlanTable
    {
        public long PlanId { get; set; }
        public Nullable<long> ProjectId { get; set; }
        public Nullable<bool> IsCharger { get; set; }
        public Nullable<bool> IsCkd { get; set; }
        public Nullable<bool> IsActive { get; set; }
        public Nullable<System.DateTime> AddadDate { get; set; }
    }
}
