using AvenaCore.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace AvenaCore.Repositories
{
    public interface INewsRepository : IBaseRepository<News>
    {
        List<News> GetByGenreId(int genreId);
        List<News> GetAllForAdmin();
        News? GetByIDForAdmin(int id);
        News? GetBySlug(string slug);
        bool SlugExists(string slug, int? exceptId = null);
        void AddLike(int newsId);
        void IncrementViews(int newsId);
        int Create(News news);
        void UpdateArticle(News news);
        void PublishArticle(int newsId, DateTime publishedAt);
    }
}
