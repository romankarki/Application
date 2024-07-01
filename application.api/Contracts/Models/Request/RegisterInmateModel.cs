using System.ComponentModel.DataAnnotations;

namespace Contracts.Models.Request
{
    public class RegisterInmateModel
    {
        [Required(ErrorMessage = "Identification Number is mandatory")]
        public string IdentificationNumber { get; set; }
        public string Name { get; set; }
        [Required(ErrorMessage = "Facility is mandatory")]
        public int FacilityId { get; set; }
        public string FacilityCode { get; set; }
        public string ContactEmail { get; set; }
        public string ContactPhone { get; set; }
        public string Delete { get; set; }
    }
}
