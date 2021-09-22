using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebChat.Utils.Common.Models.Response
{
    public class UserResponse
    {
        public int Id { get; set; }
        public string Login { get; set; }
        public string AccessToken { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
    }
}
