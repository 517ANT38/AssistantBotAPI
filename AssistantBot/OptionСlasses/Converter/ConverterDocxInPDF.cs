using PdfSharpCore.Drawing;
using PdfSharpCore.Pdf;

namespace AssistantBotAPI.OptionСlasses.Converter;

public class ConverterDocxInPDF : Converter
{
    public override string ConvertName => "docx pdf";
    public override string ConvertToType => ".pdf";
    public override async Task<string> ReadAsync(string nameFile)
    {
        using (FileStream fs = File.Open(nameFile, FileMode.Open))
        {
            if (fs.Length < 0)
                return null;
            byte[] data = new byte[fs.Length];

            await fs.ReadAsync(data, 0, data.Length);
            var s = System.Text.Encoding.UTF8.GetString(data, 0, data.Length);
            return s;
        }
        return null;
    }

    public override async Task WriteAsync(string nameFile, string? value = null)
    {
        await Task.Run(() =>
        {
            PdfDocument document = new PdfDocument();

            // And you need a page:
            PdfPage page = document.AddPage();

            // Drawing is done with an XGraphics object:
            XGraphics gfx = XGraphics.FromPdfPage(page);

            // Then you'll create a font:
            XFont font = new XFont("Verdana", 20, XFontStyle.Bold);

            FileStream fileStream = new FileStream(nameFile, FileMode.Create);
            fileStream?.Close();
            gfx.DrawString(
                value, font, XBrushes.Black,
                new XRect(0, 0, page.Width,0)
                );
            document.Save(nameFile);
        });
    }

}

