using AvenaCore.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace AvenaCore.Repositories
{
    public interface IUnitOfWork 
    {
        IUserRepository User { get; }
        IGenreRepository Genre { get; }
        INewsRepository News { get; }
        ICommentRepository Comment { get; }
    }
}
