//using Telegram.Bot;
//using Telegram.Bot.Types;

using JobWithData;
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
        var res = data.GetData();
        Console.WriteLine(String.Join("\n", res));
        return await client.SendTextMessageAsync(chatId, String.Join("\n",res), parseMode: Telegram.Bot.Types.Enums.ParseMode.Html);

        
    }

    public override string[] GetParamsArrStr(string message)
    {
        string[] tex =message.Split(',', StringSplitOptions.RemoveEmptyEntries);
        string[] res =new string[tex.Length];
        for(int i = 0; i < tex.Length; i++)
        {
            res[i]=tex[i];
        }
        
        return res;
    }
}
