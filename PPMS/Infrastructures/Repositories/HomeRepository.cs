using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using PPMS.DAL.DbModel;
using PPMS.Infrastructures.Interfaces;
using PPMS.Models;

namespace PPMS.Infrastructures.Repositories
{
    public class HomeRepository : IHomeRepository
    {
        private readonly PPMSEntities _dbContext;
        private readonly CellPhoneProjectEntities _cellPhoneProjectEntities;
        public HomeRepository()
        {
            _dbContext = new PPMSEntities();
            _dbContext.Configuration.LazyLoadingEnabled = false;

            _cellPhoneProjectEntities = new CellPhoneProjectEntities();
            _cellPhoneProjectEntities.Configuration.LazyLoadingEnabled = false;
        }

        public Tuple<string, string> GetUserRedirectionDetailsAfterAuthentication()
        {
            List<String> rolesList = new List<string>();
            List<String> roleNames = _cellPhoneProjectEntities.CmnRoles.Select(x => x.RoleName).ToList();

            String controllerName = "Home";
            String actionName = "Login";
            foreach (var name in roleNames)
            {
                if (HttpContext.Current.User.IsInRole(name))
                    rolesList.Add(name);
            }
            if (rolesList.Count != 0)
            {
                if (HttpContext.Current.User.IsInRole("PRD") || HttpContext.Current.User.IsInRole("MRI") || HttpContext.Current.User.IsInRole("TRL")
                     || HttpContext.Current.User.IsInRole("RART") || HttpContext.Current.User.IsInRole("PIQC"))
                {
                    actionName = "HandsetPlanning";
                    controllerName = "Production";
                }
                //else if (HttpContext.Current.User.IsInRole(("Wastage")))
                //{
                //    actionName = "AftersalesWastageHandset";
                //    controllerName = "WastageMgnt";

                //}
                //else if (HttpContext.Current.User.IsInRole(("CorporateStore")))
                //{
                //    actionName = "NormalHandsetReceive";
                //    controllerName = "CorporateStore";

                //}
            }
            else
            {
                controllerName = "Home";
                actionName = "Login";
            }

            return new Tuple<String, String>(actionName, controllerName);
        }

        public Boolean AuthorizedUserByUserNamePassword(string userName, string password, bool rememberMe = false)
        {
            
            var user = _cellPhoneProjectEntities.CmnUsers.FirstOrDefault(i => i.UserName == userName && i.IsActive);
            if (user != null)
            {
                string extendedRole = user.ExtendedRoleName;
                string roleName = user.RoleName;
                roleName = roleName + "," + extendedRole;
                user.RoleName = roleName;

                FormsAuthenticationTicket ticket = new FormsAuthenticationTicket(
                    1, // Ticket version
                    Convert.ToString(user.CmnUserId), // Username associated with ticket
                    DateTime.Now, // D ate/time issued
                    DateTime.Now.AddMinutes(60), // Date/time to expire
                    rememberMe, // "true" for a persistent user cookie
                    user.RoleName, // User-data, in this case the roles
                    FormsAuthentication.FormsCookiePath); // Path cookie valid for


                // Encrypt the cookie using the machine key for secure transport,
                string hash = FormsAuthentication.Encrypt(ticket);
                HttpCookie cookie = new HttpCookie(
                    FormsAuthentication.FormsCookieName, // Name of auth cookie
                    hash); // Hashed ticket

                // Set the cookie's expiration time to the tickets expiration time
                if (ticket.IsPersistent) cookie.Expires = ticket.Expiration;

                // Add the cookie to the list for outgoing response
                HttpContext.Current.Response.Cookies.Add(cookie);
                return true;
            }
            return false;
        }

        public FileContentResult GetProfilePicture(long uId)
        {
            string fileName;
            if (uId <= 0)
            {
                fileName = HttpContext.Current.Server.MapPath(@"~/assets/layouts/layout4/img/av.png");
            }
            else
            {
                using (var dbEntities = new CellPhoneProjectEntities())
                {
                    var cmnUser = (from cu in dbEntities.CmnUsers
                                   where cu.CmnUserId == uId
                                   select new CmnUserModel
                                   {
                                       CmnUserId = cu.CmnUserId,
                                       UserName = cu.UserName,
                                       UserFullName = cu.UserFullName,
                                       EmployeeCode = cu.EmployeeCode,
                                       MobileNumber = cu.MobileNumber,
                                       Email = cu.Email,
                                       RoleName = cu.RoleName,
                                       ProfilePictureUrl = cu.ProfilePictureUrl
                                   }).FirstOrDefault();

                    fileName = cmnUser != null
                        ? cmnUser.ProfilePictureUrl
                        : HttpContext.Current.Server.MapPath(@"~/assets/layouts/layout4/img/av.png");
                }
            }
            try
            {
                var fileInfo = new FileInfo(fileName);
                long imageFileLength = fileInfo.Length;
                var fs = new FileStream(fileName, FileMode.Open, FileAccess.Read);
                var br = new BinaryReader(fs);
                byte[] imageData = br.ReadBytes((int)imageFileLength);

                return new FileContentResult(imageData, "image/png");
            }
            catch (Exception)
            {
                fileName = HttpContext.Current.Server.MapPath(@"~/assets/layouts/layout4/img/av.png");
                var fileInfo = new FileInfo(fileName);
                long imageFileLength = fileInfo.Length;
                var fs = new FileStream(fileName, FileMode.Open, FileAccess.Read);
                var br = new BinaryReader(fs);
                byte[] imageData = br.ReadBytes((int)imageFileLength);
                return new FileContentResult(imageData, "image/png");
            }
        }
    }
}