using Newtonsoft.Json;
using System.Xml;

namespace AssistantBotAPI.OptionСlasses.Converter;

public class ConverterJsonInXml : Converter
{
    public override string ConvertName => "json xml";
    public override string ConvertToType => ".xml";
    public override async Task<string> ReadAsync(string nameFile)
    {
        using (FileStream fs = File.Open(nameFile, FileMode.Open))
        {
            if (fs.Length < 0)
                return null;
            byte[] data = new byte[fs.Length];

            await fs.ReadAsync(data, 0, data.Length);
            var s = System.Text.Encoding.UTF8.GetString(data, 0, data.Length);
            XmlDocument doc = JsonConvert.DeserializeXmlNode(s);
            return doc.ToString();
        }
        return null;
    }
    
}

