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
    
    public partial class Pro_Shift
    {
        public long Id { get; set; }
        public string ProductionType { get; set; }
        public string ProductName { get; set; }
        public string Month { get; set; }
        public Nullable<int> MonNum { get; set; }
        public Nullable<int> Year { get; set; }
        public string Line { get; set; }
        public string Shift_1 { get; set; }
        public string Shift_2 { get; set; }
        public string Shift_3 { get; set; }
        public string Shift_4 { get; set; }
        public string Shift_5 { get; set; }
        public string Shift_6 { get; set; }
        public string Shift_7 { get; set; }
        public Nullable<bool> IsActive { get; set; }
        public string Product { get; set; }
        public Nullable<long> Added { get; set; }
        public Nullable<System.DateTime> AddedDate { get; set; }
        public Nullable<long> Updated { get; set; }
        public Nullable<System.DateTime> UpdatedDate { get; set; }
    }
}
