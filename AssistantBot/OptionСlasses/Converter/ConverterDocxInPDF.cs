using MigraDoc.DocumentObjectModel;
using MigraDoc.Rendering;
using System.Text;

namespace AssistantBotAPI.OptionСlasses.Converter;

public class ConverterDocxInPDF : Converter
{
    public override string ConvertName => "docx pdf";
    public override string ConvertToType => ".pdf";
    public override async Task<string> ReadAsync(string nameFile)
    {
        StringBuilder textFileLines = new StringBuilder();
        using (StreamReader sr = new StreamReader(nameFile))
        {
            while (!sr.EndOfStream)
            {
                textFileLines.Append(await sr.ReadLineAsync());
                textFileLines.Append(Environment.NewLine);
            }

        }
        return textFileLines.ToString();
        
    }

    public override async Task WriteAsync(string nameFile, string? value)
    {
        await Task.Run(() =>
        {
            List<string> list = new List<string>(value.Split(Environment.NewLine));

            MigraDoc.DocumentObjectModel.Document doc = new MigraDoc.DocumentObjectModel.Document();
            Section section = doc.AddSection();


            //just font arrangements as you wish
            MigraDoc.DocumentObjectModel.Font font = new MigraDoc.DocumentObjectModel.Font("Times New Roman", 5);
            font.Bold = true;

            section.PageSetup.PageFormat = PageFormat.A4;//стандартный размер страницы
            section.PageSetup.Orientation = Orientation.Portrait;//ориентация
            section.PageSetup.BottomMargin = 10;//нижний отступ
            section.PageSetup.TopMargin = 10;//верхний отступ 
            section.PageSetup.LeftMargin = 10;
            section.PageSetup.RightMargin = 20;
            foreach (string line in list)
            {
                Paragraph paragraph = section.AddParagraph();
                paragraph.AddFormattedText(EncodingHack(line), font);

            }

            using FileStream fs = new FileStream(nameFile, FileMode.Create);
            
            PdfDocumentRenderer renderer = new PdfDocumentRenderer(false,PdfSharp.Pdf.PdfFontEmbedding.Always);
            renderer.Document = doc;
           System.Text.Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
            renderer.RenderDocument();
            renderer.Save(fs,true);
        });
    }
    private static string EncodingHack(string str)
    {
        var encoding = Encoding.BigEndianUnicode;
        var bytes = encoding.GetBytes(str);
        var sb = new StringBuilder();
        sb.Append((char)254);
        sb.Append((char)255);
        for (int i = 0; i < bytes.Length; ++i)
        {
            sb.Append((char)bytes[i]);
        }
        return sb.ToString();
    }
}

