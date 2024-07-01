using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entity
{
    public class Transfer : BaseEntity<int>
    {
        public int InmateId { get; set; }
        public int SourceFacilityId { get; set; }
        public int DestinationFacilityId { get; set; }
        public DateTime ArrivalTime { get; set; }
        public DateTime DepartureTime { get; set; }
        [ForeignKey("SourceFacilityId")]
        public virtual Facility FacilitySource { get; set; }
        [ForeignKey("DestinationFacilityId")]
        public virtual Facility FacilityDestination {get;set;}
    }
}
