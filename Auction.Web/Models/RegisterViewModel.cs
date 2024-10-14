namespace Auction.Web.Models
{
    using System.ComponentModel.DataAnnotations;

    public class RegisterViewModel
    {
        [Required(ErrorMessage ="Username is required!")]
        [StringLength(20, MinimumLength = 4, ErrorMessage = "Username must be greater than 3 but less than 20.")]
        public string Username { get; set; }

        [Required(ErrorMessage = "Password is required!")]
        [DataType(DataType.Password)]
        [StringLength(100, MinimumLength = 8, ErrorMessage = "The password must be at least 8 characters long.")]
        public string Password { get; set; }

        [Required(ErrorMessage = "Confirm Password is required!")]
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        [Display(Name = "Confirm Password")]
        public string ConfirmPassword { get; set; }

        [Required(ErrorMessage = "First Name is required!")]
        [Display(Name = "First Name")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Last Name is required!")]
        [Display(Name = "Last Name")]
        public string LastName { get; set; }
    }
}
