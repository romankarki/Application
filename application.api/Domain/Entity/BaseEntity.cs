using System.ComponentModel.DataAnnotations;

namespace Domain.Entity
{
    public class BaseEntity<T>
    {
        [Key]
        public T Id { get; set; }
        public bool IsDeleted { get; set; } 
        public T CreatedBy { get; set; }
        public T UpdatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime UpdatedDate { get; set; }

    }
}
