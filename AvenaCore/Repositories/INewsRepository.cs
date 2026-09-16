using AvenaCore.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace AvenaCore.Repositories
{
    public interface INewsRepository : IBaseRepository<News>
    {
        List<News> GetByGenreId(int genreId);
        void AddLike(int newsId);
        void IncrementViews(int newsId);
        int Create(News news);
    }
}
