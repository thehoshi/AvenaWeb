using AvenaCore.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace AvenaCore.Repositories
{
    public interface INewsRepository : IBaseRepository<News>
    {
        List<News> GetByGenreId(int genreId);
    }
}
