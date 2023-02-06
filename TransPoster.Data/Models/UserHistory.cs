using TransPoster.Data.Identity;

namespace TransPoster.Data.Models;

public class UserHistory
{
    public int Id { get; set; }
    public string HashedPassword { get; set; }
    public DateTime CreatedAt { get; set; }
    public ApplicationUser User { get; set; }

}