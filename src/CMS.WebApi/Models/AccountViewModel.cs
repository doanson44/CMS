using System;
using System.ComponentModel.DataAnnotations;

namespace CMS.WebApi.Models;

public class Login
{
    [Required]
    public string Username { get; set; }

    [Required]
    public string Password { get; set; }

    public string Code { get; set; }

    public string EmployerCode { get; set; }
}

public class ChangePassword
{
    [Required]
    public string CurrentPassword { get; set; }

    [Required]
    [DataType(DataType.Password)]
    [Display(Name = "Password")]
    public string Password { get; set; }

    [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
    public string ConfirmPassword { get; set; }
}

public class ForgotPassword
{
    [Required]
    [EmailAddress]
    public string Email { get; set; }
}

public class ResetPassword
{
    [Required]
    [EmailAddress]
    public string Email { get; set; }

    [Required]
    public string Code { get; set; }

    [Required]
    [DataType(DataType.Password)]
    [Display(Name = "Password")]
    public string Password { get; set; }

    [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
    public string ConfirmPassword { get; set; }
}

public class Validate
{
    [Required]
    [EmailAddress]
    public string Email { get; set; }
}

public class Register
{
    public int EmployerId { get; set; }

    [Required, EmailAddress]
    public string Email { get; set; }

    [Required]
    public string Password { get; set; }

    [Required]
    public string FirstName { get; set; }

    [Required]
    public string LastName { get; set; }

    public string Gender { get; set; }
    public DateTime Dob { get; set; }
    public string Phone { get; set; }

    public string StreetLine1 { get; set; }
    public string StreetLine2 { get; set; }
    public string Suburb { get; set; }
    public string PostCode { get; set; }
    public string State { get; set; }
}

public class RegisterAccount
{
    [Required]
    public string Email { get; set; }

    [Required]
    public string Password { get; set; }

    public string Name { get; set; }
}
