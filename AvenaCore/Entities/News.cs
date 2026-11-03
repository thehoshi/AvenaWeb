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
        public string? Summary { get; set; }
        public string? Slug { get; set; }
        public string Status { get; set; } = NewsStatuses.Published;
        public string? Image { get; set; }
        public int Views { get; set; }
        public int CountOfLikes { get; set; }
        public int GenreID { get; set; }
        public int? AuthorID { get; set; }
        public Author? Author { get; set; }
        public DateTime DateOfPost { get; set; }
        public DateTime? PublishedAt { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public DateTime? DeletedAt { get; set; }
    }

    public static class NewsStatuses
    {
        public const string Draft = "draft";
        public const string Published = "published";
    }
}
