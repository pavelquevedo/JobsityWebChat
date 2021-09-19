using JobsityWebChat.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace JobsityWebChat.Controllers
{
    public class BaseController : ApiController
    {
        private WebChatDBEntities _db;

        public BaseController()
        {
            this._db = new WebChatDBEntities();
        }

        public WebChatDBEntities dbContext
        {
            get
            {
                return this._db;
            }
        }
    }
}
