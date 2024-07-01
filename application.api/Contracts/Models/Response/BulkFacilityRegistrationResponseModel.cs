namespace Contracts.Models.Response
{
    public class BulkFacilityRegistrationResponseModel
    {
        public int TotalInserted { get; set; }
        public int TotalUpdated { get; set; }   
        public int TotalDeleted { get;set; }
        public List<RejectedFacilityRecords> RejectedRecords { get; set; }
    }

    public class RejectedFacilityRecords
    {
        public string FacilityCode { get; set; }
        public string Address { get; set; }
        public string ErrorMessage { get; set; }
    }

}
