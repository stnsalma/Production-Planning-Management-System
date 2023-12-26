using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using PPMS.Infrastructures.Interfaces;
using PPMS.Infrastructures.Repositories;
using PPMS.ViewModels.Home;

namespace PPMS.Controllers
{
    public class HomeController : Controller
    {

        private readonly IHomeRepository _homeRepository;

        //Inject HomeRepository into HomeController
        public HomeController(HomeRepository homeRepository)
        {
            _homeRepository = homeRepository;

        }
        [HttpGet]
        [AllowAnonymous]
        public ActionResult Login()
        {
            String userIdentity = System.Web.HttpContext.Current.User.Identity.Name;
            long userId = Convert.ToInt64(userIdentity == "" ? "0" : userIdentity);
            var tuple = _homeRepository.GetUserRedirectionDetailsAfterAuthentication();
            if (!(tuple.Item1 == "Login" && tuple.Item2 == "Home"))
            {
                return RedirectToAction(tuple.Item1, tuple.Item2);
            }
            FormsAuthentication.SignOut();

            Session.Clear();
            Session.Abandon();
            var cookie1 = new HttpCookie(FormsAuthentication.FormsCookieName, "")
            {
                Expires = DateTime.Now.AddYears(-1)
            };
            Response.Cookies.Add(cookie1);
            var cookie2 = new HttpCookie("ASP.NET_SessionId", "") { Expires = DateTime.Now.AddYears(-1) };
            Response.Cookies.Add(cookie2);

            return View();

        }
        [HttpPost]
        [AllowAnonymous]
        public ActionResult Login(VmLogin model)
        {
            if (ModelState.IsValid)
            {
                Boolean isAuthenticatedUser = _homeRepository.AuthorizedUserByUserNamePassword(model.username, model.password, model.remember);
            }
            return RedirectToAction("Login");
        }
        [HttpGet]
        [AllowAnonymous]
        public ActionResult Logout()
        {
            FormsAuthentication.SignOut();
            Session.Clear();
            Session.Abandon();
            HttpCookie cookie1 = new HttpCookie(FormsAuthentication.FormsCookieName, "");
            cookie1.Expires = DateTime.Now.AddYears(-1);
            Response.Cookies.Add(cookie1);
            HttpCookie cookie2 = new HttpCookie("ASP.NET_SessionId", "");
            cookie2.Expires = DateTime.Now.AddYears(-1);
            Response.Cookies.Add(cookie2);
            return RedirectToAction("Login");
        }
        [HttpGet]
        [AllowAnonymous]
        public ActionResult NoCookie()
        {
            return View();
        }
        public FileContentResult UserPhotos(long uuId = 0)
        {
            if (User.Identity.IsAuthenticated)
            {
                String userId = User.Identity.Name;
                string fileName;
                long uId;
                long.TryParse(userId, out uId);
                if (uuId > 0) uId = uuId;
                FileContentResult result = _homeRepository.GetProfilePicture(uId);
                return result;
            }
            else
            {
                string fileName = HttpContext.Server.MapPath(@"~/assets/layouts/layout4/img/av.png");
                var fileInfo = new FileInfo(fileName);
                long imageFileLength = fileInfo.Length;
                var fs = new FileStream(fileName, FileMode.Open, FileAccess.Read);
                var br = new BinaryReader(fs);
                byte[] imageData = br.ReadBytes((int)imageFileLength);
                return File(imageData, "image/png");
            }
        }
    }
}
