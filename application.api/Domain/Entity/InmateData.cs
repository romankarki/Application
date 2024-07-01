using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entity;

public class InmateData : BaseEntity<int>
{
    public string IdentificationNumber { get; set; }
    public string Name { get; set; }
    public int CurrentFacility { get; set; }
    public string ContactEmail { get; set; }
    public string ContactPhone { get; set; }
    [ForeignKey("CurrentFacility")]
    public virtual Facility Facility { get; set; }
}