using AssistantBotAPI.Models;
using Newtonsoft.Json.Linq;
using System.Diagnostics;

namespace AssistantBotAPI
{
    public class AssistantBot
    {


        static async Task Main(String[] args)
        {
            
            var text = File.ReadAllText(@"./MyBot.json");
            var json = JObject.Parse(text);
            Bot bot = new Bot(json["Token"].ToString(), json["Name"].ToString());
            await bot.startBot();
            
        }

    }
}
