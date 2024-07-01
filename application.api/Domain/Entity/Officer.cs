namespace Domain.Entity
{
    public class Officer : BaseEntity<int>
    {
        public string IdentificationNumber { get; set; }
        public string Name { get; set;}
        public string Password { get; set;}
        public string ContactEmail { get; set;}
        public string ContactNumber { get; set;}
    }
}
