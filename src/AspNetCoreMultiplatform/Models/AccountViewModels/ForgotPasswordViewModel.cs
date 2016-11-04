using System.ComponentModel.DataAnnotations;

namespace AspNetCoreMultiplatform.Models.AccountViewModels
{
    public class ForgotPasswordViewModel
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
    }
}
