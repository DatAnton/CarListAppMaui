using SQLite;

namespace CarListApp.Models
{
    [Table("Cars")]
    public class Car : BaseEntity
    {
        public string Make { get; set; }
        public string Model { get; set; }
        [MaxLength(12), Unique]
        public string Vin { get; set; }
    }
}
