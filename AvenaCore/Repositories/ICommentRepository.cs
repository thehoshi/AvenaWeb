using AvenaCore.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace AvenaCore.Repositories
{
    public interface ICommentRepository : IBaseRepository<Comment>
    {
        List<Comment> GetByNewsID(int newsId);
    }
}
