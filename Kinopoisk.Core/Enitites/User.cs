using Microsoft.AspNetCore.Identity;

namespace Kinopoisk.Core.Enitites;

public class User : IdentityUser<int>
{
    public List<Comment> Comments { get; set; }
    public List<Rating> Ratings { get; set; }

    public User()
    {
        Comments = new();
        Ratings = new();
    }
}
