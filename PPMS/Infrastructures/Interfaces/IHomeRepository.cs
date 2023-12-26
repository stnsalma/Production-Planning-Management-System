using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PPMS.Infrastructures.Interfaces
{
    public interface IHomeRepository
    {
        #region CommnonUserRouteIdentifier
        Tuple<String, String> GetUserRedirectionDetailsAfterAuthentication();
        Boolean AuthorizedUserByUserNamePassword(string username, string password, bool remember);

        #endregion

        FileContentResult GetProfilePicture(long uId);
    }
}