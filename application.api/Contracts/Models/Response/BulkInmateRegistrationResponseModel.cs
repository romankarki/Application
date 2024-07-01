namespace Contracts.Models.Response
{
    public class BulkInmateRegistrationResponseModel
    {
        public int TotalInserted { get; set; }
        public int TotalUpdated { get; set; }   
        public int TotalDeleted { get;set; }
        public List<RejectedInmateRecords> RejectedRecords { get; set; }
    }

    public class RejectedInmateRecords
    {
        public string FacilityCode { get; set; }
        public string IdentificationNumber { get; set; }
        public string ErrorMessage { get; set; }
    }

}
