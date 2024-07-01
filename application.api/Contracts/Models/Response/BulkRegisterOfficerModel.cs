namespace Contracts.Models.Response
{
    public class BulkRegisterOfficerModel
    {
        public int TotalInserted { get; set; }
        public int TotalUpdated { get; set; }   
        public int TotalDeleted { get;set; }
        public List<RejectedRecords> RejectedRecords { get; set; }
    }

    public class RejectedRecords
    {
        public string IdentificationNumber { get; set; }
        public string ErrorMessage { get; set; }
    }
        

}
