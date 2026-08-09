using System;
using System.Collections.Generic;
using System.Text;

namespace AvenaCore.Repositories
{
    public interface IBaseRepository<T>
    {
        List<T> GetAll();
        T GetById(int id);
        void Insert(T entity);
        void Update(T entity);

    }
}
