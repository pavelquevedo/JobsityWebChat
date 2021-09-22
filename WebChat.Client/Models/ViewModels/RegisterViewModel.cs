using System.ComponentModel.DataAnnotations;

namespace WebChat.Client.Models.ViewModels
{
    public class RegisterViewModel
    {
        [Required]
        [StringLength(20)]
        public string Login { get; set; }
        [Required]
        [StringLength(20)]
        public string Password { get; set; }
        [Required]
        [StringLength(20)]
        [Compare("Password")]
        [Display(Name = "Confirm Password")]
        public string ConfirmPassword { get; set; }
        [Required]
        [StringLength(50)]
        [Display(Name = "First Name")]
        public string FirstName { get; set; }
        [Required]
        [StringLength(50)]
        [Display(Name = "Last Name")]
        public string LastName { get; set; }

    }
}