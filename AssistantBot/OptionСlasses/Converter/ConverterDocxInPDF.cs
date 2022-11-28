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
            MigraDoc.DocumentObjectModel.Font font = new MigraDoc.DocumentObjectModel.Font("Times New Roman", 15);
            font.Bold = true;

            //add each line to pdf 

            foreach (string line in list)
            {
                Paragraph paragraph = section.AddParagraph();
                paragraph.AddFormattedText(line, font);

            }

            FileStream fs = new FileStream(nameFile, FileMode.Create);
            
            PdfDocumentRenderer renderer = new PdfDocumentRenderer();
            renderer.Document = doc;
            System.Text.Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
            renderer.RenderDocument();
            renderer.Save(fs,true);
        });
    }

}

