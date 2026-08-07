using AvenaCore.Entities.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace AvenaCore.Entities
{
    public class User : IDBEntity
    {
        public int ID { get; set; }
        public required string Username { get; set; }
        public required string Name { get; set; }
        public required string Password { get; set; }
        public required string AvatarImg { get; set; }
    }
}
