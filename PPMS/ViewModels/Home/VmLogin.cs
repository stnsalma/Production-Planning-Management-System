using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PPMS.ViewModels.Home
{
    public class VmLogin
    {
        public String username { get; set; }
        public String password { get; set; }
        public Boolean remember { get; set; }
    }
}