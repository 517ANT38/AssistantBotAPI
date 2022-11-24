using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

using System.Net.Http.Json;
using Newtonsoft.Json;
using GTranslatorAPI;

namespace OptionСlasses.Translator;

public class TranslatorBot
{
    
    
    private Translation objectFor;
    public TranslatorBot(string text,string lng1,string lng2)
    {
        this.objectFor=new Translation();
        this.objectFor.OriginalText=text;
    }
    public async Task<string> GetTranslation()
    {
        //HttpClient client = new HttpClient();
        ////JsonContent content =  JsonContent.Create(objectFor);

        //var json = JsonConvert.SerializeObject(objectFor);
        //var content = new StringContent(json, Encoding.UTF8, "application/json");
        //var response = await client.PostAsync("https://translate.argosopentech.com/translate", content);
        //string a= await response.Content.ReadAsStringAsync();
        //return a;
    }

}
