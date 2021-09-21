using System.ComponentModel.DataAnnotations;

namespace WebChat.Utils.Common.Models.Request
{
    public class LoginRequest
    {
        [Required]
        [StringLength(20)]
        public string Login { get; set; }
        [Required]
        public string Password { get; set; }
    }
}
