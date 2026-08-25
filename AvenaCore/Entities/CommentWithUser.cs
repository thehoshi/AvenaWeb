namespace AvenaCore.Entities;

public class CommentWithUser
{
    public int ID { get; set; }
    public int NewsID { get; set; }
    public int UserID { get; set; }

    public string Text { get; set; } = "";

    public int CountOfLikes { get; set; }
    public DateTime DateOfPost { get; set; }

    public string Username { get; set; } = "";
    public string Name { get; set; } = "";
}