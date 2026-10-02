using AvenaCore.Entities.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace AvenaCore.Entities
{
    public class News : IDeletableEntity
    {
        public int ID { get; set; }
        public required string Title { get; set; }
        public required string Info { get; set; }
        public string? Image { get; set; }
        public int Views { get; set; }
        public int CountOfLikes { get; set; }
        public int GenreID { get; set; }
        public int? AuthorID { get; set; }
        public User? Author { get; set; }
        public DateTime DateOfPost { get; set; }
        public DateTime? DeletedAt { get; set; }
    }
}
