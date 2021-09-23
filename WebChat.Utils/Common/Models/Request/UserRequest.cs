using System.ComponentModel.DataAnnotations;

namespace WebChat.Utils.Common.Models.Request
{
    public class UserRequest
    {
        [Required]
        [StringLength(20)]
        public string Login { get; set; }
        [Required]
        public string Password { get; set; }
        [Required]
        [StringLength(20)]
        public string FirstName { get; set; }
        [StringLength(20)]
        public string LastName { get; set; }
    }
}
