using Microsoft.AspNetCore.Identity;

namespace Kinopoisk.Core.Enitites;

public class User : IdentityUser<int>
{
    public virtual List<Comment> Comments { get; set; }
    public virtual List<Rating> Ratings { get; set; }

    public User()
    {
        Comments = new();
        Ratings = new();
    }
}
