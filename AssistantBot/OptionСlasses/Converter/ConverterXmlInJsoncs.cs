using Newtonsoft.Json;
using System.Xml;

namespace AssistantBotAPI.OptionСlasses.Converter;

public class ConverterXmlInJsoncs : Converter
{
    public override string ConvertName => "xml json";
    public override string ConvertToType => ".json";
    public override async Task<string> ReadAsync(string nameFile)
    {
        using StreamReader fs =new StreamReader(nameFile);
       //Console.WriteLine(nameFile);
        var s=await fs.ReadToEndAsync();
        
        //var s = System.Text.Encoding.UTF8.GetString(data, 0, data.Length);
        XmlDocument doc = new XmlDocument();
        doc.LoadXml(s);
        
        return doc.OuterXml;
        
        
    }

    public override async Task WriteAsync(string nameFile, string? value)
    {
        using (StreamWriter fs = new StreamWriter(nameFile)) 
        {
            XmlDocument doc = new XmlDocument();
            doc.LoadXml(value);

            string jsonText = JsonConvert.SerializeXmlNode(doc,Newtonsoft.Json.Formatting.Indented);
            await fs.WriteAsync(jsonText);
        }
        
        

    }

    
}

