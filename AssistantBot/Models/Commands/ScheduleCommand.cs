//using Telegram.Bot;
//using Telegram.Bot.Types;

using JobWithData;
using OptionСlasses.SaveUpadateSheduleDB;
using OptionСlasses.Shedule;
using System.Text.RegularExpressions;
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

        

        WrapperSchedule schedule = new WrapperSchedule(arr);
        WrapperAboveData<string> data =  await schedule.LoadData();
        var res = String.Join("\n", data.GetData());
        SUSheduleDb sU = new SUSheduleDb(chatId, res);
        sU.paradSetOfTimeClearShed();

        return await client.SendTextMessageAsync(chatId, res, parseMode: Telegram.Bot.Types.Enums.ParseMode.Html);

        
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
