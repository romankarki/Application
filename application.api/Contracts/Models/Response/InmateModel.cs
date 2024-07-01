namespace Contracts.Models.Response
{
    public class InmateModel
    {
        public int Id { get; set; }
        public string IdentificationNumber { get; set; }
        public string InmateName { get; set; }
        public string FacilityName { get; set; }
        public string FacilityCode { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
    }
}
