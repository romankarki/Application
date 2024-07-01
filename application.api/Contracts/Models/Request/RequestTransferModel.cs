using System.ComponentModel.DataAnnotations;

namespace Contracts.Models.Request
{
    public class RequestTransferModel
    {
        public List<string> IdentificationNumbers { get; set; }
        [Required(ErrorMessage = "Destination Facility is mandatory")]
        public int DestinationFacilityId { get; set;}
        public DateTime ArrivalTime { get; set; }
        public DateTime DepartureTime { get; set; }
    }
}
