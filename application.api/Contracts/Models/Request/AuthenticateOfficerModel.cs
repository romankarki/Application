using System.ComponentModel.DataAnnotations;

namespace Contracts.Models.Request
{
    public class AuthenticateOfficerModel
    {
        [Required(ErrorMessage = "Identification Number is mandatory")]
        public string IdentificationNumber { get; set; }
        [Required(ErrorMessage = "Password is mandatory")]
        public string Password { get; set; }
    }
}
