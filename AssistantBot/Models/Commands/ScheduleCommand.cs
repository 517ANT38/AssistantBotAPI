//using Telegram.Bot;
//using Telegram.Bot.Types;

using AssistantBotAPI.OptionСlasses.Shedule;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace AssistantBotAPI.Models.Commands;
internal class ScheduleCommand : Command
{
    public override string Name => "/schedule";
    
    public override bool Contains(string message)
    {
        return message.Contains(this.Name);
    }

    public override async Task<Message> Execute(long chatId, TelegramBotClient client, params string[] arr)
    {

        IAsyncLoaDatable wrapShedSetGet=new WrapShedSetGetDbClass(chatId, arr);
        var res = await wrapShedSetGet.LoadData();




        return await client.SendTextMessageAsync(chatId, res.Item2, parseMode: Telegram.Bot.Types.Enums.ParseMode.Html);

        
    }

    //public override string[] GetParamsArrStr(string message)
    //{
    //    Regex regex = new Regex($"{Name}");
    //    string[] tex =regex.Split(message)[1].Split(",", StringSplitOptions.RemoveEmptyEntries);
    //    for (int i = 0; i < tex.Length; i++)
    //    {
    //        tex[i] = tex[i].Trim();
    //    }
    //    return tex;
    //}
}
