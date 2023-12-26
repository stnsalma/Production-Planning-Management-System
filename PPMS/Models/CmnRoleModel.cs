using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PPMS.Models
{
    public class CmnRoleModel
    {
        public long CmnRoleId { get; set; }
        public string RoleName { get; set; }
        public long? Added { get; set; }
        public DateTime? AddedDate { get; set; }
        public long? Updated { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public string RoleDescription { get; set; }
        public bool? IsHead { get; set; }
    }
}