using System;
using System.Collections.Generic;
using System.Text;

namespace AvenaCore.Entities.Interfaces
{
    public interface IDeletableEntity : IDBEntity
    {
        DateTime? DeletedAt { get; set; }
    }
}
