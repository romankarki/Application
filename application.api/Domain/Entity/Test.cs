using System.ComponentModel.DataAnnotations;

namespace Domain.Entity;

public class Test
{
    [Key]
    public int Id;
    public string Name;
}