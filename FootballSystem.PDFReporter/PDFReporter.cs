namespace FootballSystem.PDFReporter
{
    using System.IO;
    using System.Linq;

    using Data;

    using iTextSharp.text;
    using iTextSharp.text.pdf;

    public class PDFReporter
    {
        /// <summary>
        /// Generates player data report in the bin\debug folder.
        /// </summary>
        public static void GenerateReport()
        {
            FileStream fs = new FileStream("FootbalSystemReport.pdf", FileMode.Create);
            Document doc = new Document();
            PdfWriter.GetInstance(doc, fs);

            PdfPTable table = new PdfPTable(4);

            table.WidthPercentage = 80;

            // First topic cell
            table.AddCell(new PdfPCell(new Phrase("Football Players", new Font(Font.FontFamily.COURIER, 15, 2, BaseColor.WHITE))) { HorizontalAlignment = Element.ALIGN_CENTER, PaddingBottom = 10, BackgroundColor = BaseColor.RED, Colspan = 4 });

            // Header cells
            table.AddCell(new PdfPCell(new Phrase("Player Name", new Font(Font.FontFamily.COURIER, 8, 1, BaseColor.BLACK))) { HorizontalAlignment = Element.ALIGN_CENTER, Padding = 5, BackgroundColor = new BaseColor(System.Drawing.Color.Silver) });
            table.AddCell(new PdfPCell(new Phrase("Player Age", new Font(Font.FontFamily.COURIER, 8, 1, BaseColor.BLACK))) { HorizontalAlignment = Element.ALIGN_CENTER, Padding = 5, BackgroundColor = new BaseColor(System.Drawing.Color.Silver) });
            table.AddCell(new PdfPCell(new Phrase("Team", new Font(Font.FontFamily.COURIER, 8, 1, BaseColor.BLACK))) { HorizontalAlignment = Element.ALIGN_CENTER, Padding = 5, BackgroundColor = new BaseColor(System.Drawing.Color.Silver) });
            table.AddCell(new PdfPCell(new Phrase("Salary", new Font(Font.FontFamily.COURIER, 8, 1, BaseColor.BLACK))) { HorizontalAlignment = Element.ALIGN_CENTER, Padding = 5, BackgroundColor = new BaseColor(System.Drawing.Color.Silver) });

            var db = new FootballDbContext();
            var playersQuery = db.Players.Select(pl => new
            {
                FullName = pl.FirstName + " " + pl.LastName,
                Age = pl.Age,
                Salary = pl.Salary,
                TeamName = pl.Team.Name
            });

            foreach (var player in playersQuery)
            {
                table.AddCell(new PdfPCell(new Phrase($"{player.FullName}", new Font(Font.FontFamily.COURIER, 8, 1, BaseColor.RED))) { HorizontalAlignment = Element.ALIGN_CENTER, Padding = 5, BackgroundColor = BaseColor.WHITE });
                table.AddCell(new PdfPCell(new Phrase($"{player.Age}", new Font(Font.FontFamily.COURIER, 8, 1, BaseColor.RED))) { HorizontalAlignment = Element.ALIGN_CENTER, Padding = 5, BackgroundColor = BaseColor.WHITE });
                table.AddCell(new PdfPCell(new Phrase($"{player.TeamName}", new Font(Font.FontFamily.COURIER, 8, 1, BaseColor.RED))) { HorizontalAlignment = Element.ALIGN_CENTER, Padding = 5, BackgroundColor = BaseColor.WHITE });
                table.AddCell(new PdfPCell(new Phrase($"{player.Salary}", new Font(Font.FontFamily.COURIER, 8, 1, BaseColor.RED))) { HorizontalAlignment = Element.ALIGN_CENTER, Padding = 5, BackgroundColor = BaseColor.WHITE });
            }

            doc.Open();
            doc.Add(table);
            doc.Close();
        }
    }
}
