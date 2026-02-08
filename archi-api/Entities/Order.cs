using System.ComponentModel.DataAnnotations;

namespace Archi.Entities;

public class Order
{
    [Key]
    public int Id { get; set; }
    public string Name { get; set; }
    
}
