using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebChat.Utils.Common.Models.Response;

namespace WebChat.Client.Controllers
{
    public class BaseController : Controller
    {
        private UserResponse _userSession;

        public UserResponse UserSession
        {
            get
            {
                if (Session["User"] != null)
                {
                    this._userSession = (UserResponse)Session["User"];
                }
                return this._userSession;
            }
        }
    }
}