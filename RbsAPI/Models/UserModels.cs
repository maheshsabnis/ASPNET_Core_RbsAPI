using System.ComponentModel.DataAnnotations;

namespace RbsAPI.Models
{
    /// <summary>
    /// Class for Login Information
    /// </summary>
    public class LoginUser
    {
        [Required(ErrorMessage = "User Name is Required")]
        public string UserName { get; set; }

        [Required(ErrorMessage = "Password is Required")]
        public string Password { get; set; }
         
    }

    /// <summary>
    /// Class for Registering User
    /// </summary>
    public class RegisterUser
    {
        [Required(ErrorMessage = "Email is Required")]
        [EmailAddress]
        public string Email { get; set; }
        [Required(ErrorMessage = "Password is Required")]
        [RegularExpression("^((?=.*?[A-Z])(?=.*?[a-z])(?=.*?[0-9])|(?=.*?[A-Z])(?=.*?[a-z])(?=.*?[^a-zA-Z0-9])|(?=.*?[A-Z])(?=.*?[0-9])(?=.*?[^a-zA-Z0-9])|(?=.*?[a-z])(?=.*?[0-9])(?=.*?[^a-zA-Z0-9])).{8,}$",
            ErrorMessage = "Passwords must be minimum 8 characters and can contain upper case, lower case, number (0-9) and special character")]
        public string Password { get; set; }
        [Compare("Password")]
        public string ConfirmPassword { get; set; }
    }

    public class ApplicationRole
    {
        public string Name { get; set; }
        public string NormalizedName { get; set; }
    }

    /// <summary>
    /// Class for Approving User to Assign Role to it
    /// </summary>
    public class UserRole
    {
        public string UserName { get; set; }
        public string RoleName { get; set; }
    }

    public class Users
    {
        public string Email { get; set; }
        public string UserName { get; set; }
    }
}
