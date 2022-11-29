using Newtonsoft.Json;
using System.Xml;

namespace AssistantBotAPI.OptionСlasses.Converter;

public class ConverterJsonInXml : Converter
{
    public override string ConvertName => "json xml";
    public override string ConvertToType => ".xml";
    public override async Task<string> ReadAsync(string nameFile)
    {
       // Console.WriteLine(nameFile);
        using FileStream fs = File.Open(nameFile, FileMode.Open);
        
        if (fs.Length < 0)
            return null;
        byte[] data = new byte[fs.Length];

        await fs.ReadAsync(data, 0, data.Length);
        var s = System.Text.Encoding.UTF8.GetString(data, 0, data.Length);
        
        XmlDocument doc = JsonConvert.DeserializeXmlNode(s,"root");
        return doc.OuterXml;
        
        
    }

    public override async Task WriteAsync(string nameFile, string? value)
    {
        XmlDocument doc = new XmlDocument();
        doc.LoadXml(value);
        XmlWriterSettings? settings = new XmlWriterSettings();
        settings.Indent = true;
        settings.ConformanceLevel = ConformanceLevel.Fragment;
        StreamWriter sw = new StreamWriter(nameFile);
        using (XmlWriter writer = XmlWriter.Create(sw, settings))
        {
           doc.WriteTo(writer);
        }
        sw.Close();
    }
}

