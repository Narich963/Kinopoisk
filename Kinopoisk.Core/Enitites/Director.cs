using System.ComponentModel.DataAnnotations;

namespace Kinopoisk.Core.Enitites;

public class Director
{
    public int Id { get; set; }

    [MaxLength(50)]
    public string Name { get; set; }
}
