using AvenaCore.Entities.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace AvenaCore.Entities
{
    public class Genre : IDBEntity
    {
        public int ID { get; set; }
        public required string NameGenre { get; set; }
    }
}
