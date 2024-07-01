namespace Domain.Entity
{
    public class Facility : BaseEntity<int>
    {
        public string Address { get; set; }
        public string FacilityCode { get; set; }
        public string FacilityName { get; set; }
        public string FacilityContactEmail { get; set; }
        public string FacilityContactNumber { get; set; }
        public string FacilityContactPerson { get; set; }
    }
}
