using AvenaCore.Entities.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace AvenaCore.Entities
{
    public class Comment : IDBEntity
    {
        public int ID { get; set; }
        public int NewsID { get; set; }
        public int UserID { get; set; }
        public required string Text { get; set; }
        public int CountOfLikes { get; set; }
        public DateTime DateOfPost { get; set; }

    }
}
