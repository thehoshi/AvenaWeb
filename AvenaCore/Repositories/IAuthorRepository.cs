using AvenaCore.Entities;

namespace AvenaCore.Repositories;

public interface IAuthorRepository
{
    List<Author> GetAll();
    Author? GetByID(int id);
}
