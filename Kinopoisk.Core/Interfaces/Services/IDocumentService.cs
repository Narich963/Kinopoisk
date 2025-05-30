using Kinopoisk.Core.DTO;
using QuestPDF.Infrastructure;

namespace Kinopoisk.Core.Interfaces.Services;

public interface IDocumentService : IDocument
{
    static List<FilmDTO> Films { get; set; }
    byte[] ExportToExcel();
}
