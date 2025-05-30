using ClosedXML.Excel;
using Kinopoisk.Core.DTO;
using Kinopoisk.Core.Interfaces.Services;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;

namespace Kinopoisk.Services.Services;

public class DocumentService : IDocumentService
{
    public static List<FilmDTO> Films { get; set; }

    public void Compose(IDocumentContainer container)
    {
        container
        .Page(page =>
        {
            page.Margin(30);
            page.Size(PageSizes.A4.Landscape());

            page.Content().Column(column =>
            {
                column.Item().PaddingBottom(10).Text("Films List").FontSize(20).Bold();

                column.Item().Table(table =>
                {
                    table.ColumnsDefinition(columns =>
                    {
                        columns.RelativeColumn(1); // ID
                        columns.RelativeColumn(2); // Name
                        columns.RelativeColumn(2); // Publish Date
                        columns.RelativeColumn(2); // Country
                        columns.RelativeColumn(1); // IMDB Rating
                        columns.RelativeColumn(1); // Sites Rating
                        columns.RelativeColumn(2); // Genres
                        columns.RelativeColumn(3); // Actors
                        columns.RelativeColumn(2); // Director
                    });

                    table.Header(header =>
                    {
                        void AddHeaderCell(string text) =>
                            header.Cell().Element(c =>
                                c.Background(Colors.Grey.Lighten3)
                                 .Border(1)
                                 .BorderColor(Colors.Black)
                                 .Padding(5)
                                 .AlignCenter()
                            ).Text(text).Bold();

                        AddHeaderCell("ID");
                        AddHeaderCell("Name");
                        AddHeaderCell("Publish Date");
                        AddHeaderCell("Country");
                        AddHeaderCell("IMDB Rating");
                        AddHeaderCell("Sites Rating");
                        AddHeaderCell("Genres");
                        AddHeaderCell("Actors");
                        AddHeaderCell("Director");
                    });

                    int rowIndex = 0;
                    foreach (var film in Films)
                    {
                        bool isEven = rowIndex++ % 2 == 0;
                        string rowColor = isEven ? Colors.White : Colors.Grey.Lighten4;

                        table.Cell().Element(c => CellStyle(c, rowColor)).Text(film.Id.ToString());
                        table.Cell().Element(c => CellStyle(c, rowColor)).Text(film.Name);
                        table.Cell().Element(c => CellStyle(c, rowColor)).Text(film.PublishDate.ToShortDateString());
                        table.Cell().Element(c => CellStyle(c, rowColor)).Text(film.Country?.Name ?? "Unknown");
                        table.Cell().Element(c => CellStyle(c, rowColor)).Text(film.IMDBRating?.ToString("0.0") ?? "N/A");
                        table.Cell().Element(c => CellStyle(c, rowColor)).Text(film.SitesRating.ToString("0.0"));
                        table.Cell().Element(c => CellStyle(c, rowColor)).Text(string.Join(", ", film.Genres.Select(g => g.Genre?.Name ?? "Unknown")));
                        table.Cell().Element(c => CellStyle(c, rowColor)).Text(string.Join(", ", film.Employees.Where(e => !e.IsDirector).Select(e => e.FilmEmployee?.Name ?? "Unknown")));
                        table.Cell().Element(c => CellStyle(c, rowColor)).Text(film.Employees.FirstOrDefault(e => e.IsDirector)?.FilmEmployee?.Name ?? "Unknown");
                    }
                });
            });
        });
    }
    static IContainer CellStyle(IContainer container, string backgroundColor) =>
    container
        .Background(backgroundColor)
        .Border(1)
        .BorderColor(Colors.Grey.Lighten2)
        .Padding(5)
        .AlignMiddle()
        .AlignLeft();

    public byte[] ExportToExcel()
    {
        using var workbook = new XLWorkbook();
        var worksheet = workbook.Worksheets.Add("Films");

        worksheet.Cell(1, 1).Value = "ID";
        worksheet.Cell(1, 2).Value = "Name";
        worksheet.Cell(1, 3).Value = "Publish Date";
        worksheet.Cell(1, 4).Value = "Country";
        worksheet.Cell(1, 5).Value = "IMDB Rating";
        worksheet.Cell(1, 6).Value = "Sites Rating";
        worksheet.Cell(1, 7).Value = "Genres";
        worksheet.Cell(1, 8).Value = "Actors";
        worksheet.Cell(1, 9).Value = "Director";

        for (int i = 0; i < Films.Count; i++)
        {
            var film = Films[i];
            int row = i + 2;

            worksheet.Cell(row, 1).Value = film.Id;
            worksheet.Cell(row, 2).Value = film.Name;
            worksheet.Cell(row, 3).Value = film.PublishDate.ToShortDateString();
            worksheet.Cell(row, 4).Value = film.Country?.Name ?? "Unknown";
            worksheet.Cell(row, 5).Value = film.IMDBRating ?? 0;
            worksheet.Cell(row, 6).Value = film.SitesRating;
            worksheet.Cell(row, 7).Value = string.Join(", ", film.Genres.Select(g => g.Genre?.Name ?? "Unknown"));
            worksheet.Cell(row, 8).Value = string.Join(", ", film.Employees.Where(e => !e.IsDirector).Select(e => e.FilmEmployee?.Name ?? "Unknown"));
            worksheet.Cell(row, 9).Value = film.Employees.FirstOrDefault(e => e.IsDirector)?.FilmEmployee?.Name ?? "Unknown";
        }
        using var stream = new MemoryStream();
        workbook.SaveAs(stream);
        return stream.ToArray();
    }
}
