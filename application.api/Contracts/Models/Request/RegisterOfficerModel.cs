using System.ComponentModel.DataAnnotations;

namespace Contracts.Models.Request
{
    public class RegisterOfficerModel
    {
        public string IdentificationNumber { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string Name { get; set; }
        public string Delete { get; set; }
    }
}
