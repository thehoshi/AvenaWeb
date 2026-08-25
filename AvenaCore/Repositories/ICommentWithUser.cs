using AvenaCore.Entities;
using AvenaCore.Repositories;

public interface ICommentRepository : IBaseRepository<Comment>
{
    List<Comment> GetByNewsID(int newsId);
    List<CommentWithUser> GetByNewsIdWithUser(int newsId);
}