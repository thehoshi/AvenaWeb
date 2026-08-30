using AvenaCore.Entities;

namespace AvenaCore.Repositories
{
    public interface IUserRepository : IBaseRepository<User>
    {
        User? GetByUsername(string username);
        void UpdateProfile(int id, string name, string avatarImg);
    }
}