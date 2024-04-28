using System.ComponentModel.DataAnnotations;
namespace Kindergarten.Models
{
    public class UserLoginModel
    {
        [Required(ErrorMessage = "{0} Required ")]
        [Display(Name = "Username")]
        [StringLength(30,ErrorMessage ="Username Must be under 30 characters")]
        public string Username { get; set; } = null!;
        [Required(ErrorMessage = "{0} Required ")]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        [StringLength(30, ErrorMessage = "Username Must be under 30 characters")]
        public string Password { get; set; } = null!;
        [Display(Name = "Remember Me?")]
        public bool RememberMe { get; set; }
        public string? ReturnUrl { get; set; }
    }
}
