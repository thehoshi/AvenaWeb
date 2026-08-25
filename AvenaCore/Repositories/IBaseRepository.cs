using System;
using System.Collections.Generic;
using System.Text;

namespace AvenaCore.Repositories
{
    public interface IBaseRepository<T>
    {
        List<T> GetAll();
        void Insert(T entity);
        T? GetByID(int id);
    }
}
