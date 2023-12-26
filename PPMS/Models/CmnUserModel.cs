using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PPMS.Models
{
    public class CmnUserModel
    {
        public long CmnUserId { get; set; }
        public string UserFullName { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string MobileNumber { get; set; }
        public string Email { get; set; }
        public string ProfilePictureUrl { get; set; }
        public string Designation { get; set; }
        public string EmployeeCode { get; set; }
        public bool IsActive { get; set; }
        public string RoleName { get; set; }
        public long? AssignBy { get; set; }
        public DateTime? AssignDate { get; set; }
        public DateTime? AssignStartDate { get; set; }
        public DateTime? AssignEndDate { get; set; }
        public string AssignRoles { get; set; }
        public string Department { get; set; }
        public string Section { get; set; }
        public DateTime? DateOfJoining { get; set; }
        public string Status { get; set; }
        public decimal? ServiceLength { get; set; }
        public long? Added { get; set; }
        public DateTime? AddedDate { get; set; }
        public long? Updated { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public string ExtendedRoleName { get; set; }
        public DateTime? ExpiryDate { get; set; }
        public string LastLoginDateTime { get; set; }
        public bool? IsRememberMailSend { get; set; }
    }
}