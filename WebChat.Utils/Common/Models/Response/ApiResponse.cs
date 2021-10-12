using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace WebChat.Utils.Common.Models.Response
{
    /// <summary>
    /// Model to return dynamic api responses
    /// </summary>
    public class ApiResponse
    {
        public HttpStatusCode StatusCode { get; set; }
        public object Content { get; set; }
    }
}
