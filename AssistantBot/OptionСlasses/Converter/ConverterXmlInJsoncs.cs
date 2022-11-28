using Newtonsoft.Json;
using System.Xml;

namespace AssistantBotAPI.OptionСlasses.Converter;

public class ConverterXmlInJsoncs : Converter
{
    public override string ConvertName => "xml json";
    public override string ConvertToType => ".json";
    public override async Task<string> ReadAsync(string nameFile)
    {
        using (FileStream fs = File.Open(nameFile, FileMode.Open))
        {
            if (fs.Length < 0)
                return null;
            byte[] data = new byte[fs.Length];

            await fs.ReadAsync(data, 0, data.Length);
            var s = System.Text.Encoding.UTF8.GetString(data, 0, data.Length);
            XmlDocument doc = new XmlDocument();
            doc.LoadXml(s);
            return JsonConvert.SerializeXmlNode(doc);
        }
        return null;
    }

    //public override async Task WriteAsync(string nameFile, string? value = null)
    //{
    //    using (FileStream fs = File.Open(nameFile, FileMode.Create))
    //    {

            
    //        var b = System.Text.Encoding.UTF8.GetBytes(value);

    //        await fs.WriteAsync(b, 0, value.Length);
    //    }
    //}
}

