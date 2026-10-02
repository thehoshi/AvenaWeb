using AvenaCore.Entities.Interfaces;

namespace AvenaCore.Entities
{
    public class Author : IDBEntity
    {
        public int ID { get; set; }
        public required string FullName { get; set; }
        public string? Role { get; set; }
        public string? Affiliation { get; set; }
        public string? Location { get; set; }
    }
}
