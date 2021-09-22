using System.Web.Http;
using WebChat.Api.Models;

namespace WebChat.Api.Controllers
{
    [Authorize]
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
