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
    public override List<string> RegStringChekData
    {
        get
        {
            List<string> ts = new List<string>()
            {
                @"^(?:(http(s|)|ftp)?:\/\/)[\w.-]+(?:\.[\w\.-]+)+[\w\-\._~:\/?#[\]@!\$&'\(\)\*\+,;=.]+$",
                @"^(б|м|с)[1-5]{0,1}-([^\w\sа-яЙйЫЪЬ]{3,4}(оз|з|ипу|озипу|))-[1-5]{2}(| )",
               @"[Нн]а сегодня"
            };
            return ts;
        }
    }
    public override bool Contains(string message)
    {
        return message.Contains(this.Name);
    }

    public override async Task<Message> Execute(long chatId, TelegramBotClient client, params string[] arr)
    {

        string? url = null;
        string? group = null;
        bool fl=true;
        
        
        try
        {
            
           
            if (Regex.IsMatch(arr[0], RegStringChekData[0], RegexOptions.IgnoreCase))
            {
                url = arr[0];
            }
            else
                await client.SendTextMessageAsync(
                    chatId, 
                    "Неправильный адрес сайта", 
                    parseMode: Telegram.Bot.Types.Enums.ParseMode.Html);

            if (Regex.IsMatch(arr[1], RegStringChekData[1], RegexOptions.ECMAScript))
            {
                group = arr[1];
            }
            else
                await client.SendTextMessageAsync(
                    chatId,
                    "Неправильное формат название группы ",
                    parseMode: Telegram.Bot.Types.Enums.ParseMode.Html);

            group = arr[1];
            string tmp = string.Join(" ", arr[2], arr[3]);
            fl = !Regex.IsMatch(tmp, RegStringChekData[2], RegexOptions.IgnoreCase); 
            
            

        }
        catch (IndexOutOfRangeException ex)
        {
            Console.WriteLine(ex);
        }
        if (group == null|| url==null)
        {
            return await client.SendTextMessageAsync(chatId, "Запустите команду еще раз,назвав группу и адрес сайта ", parseMode: Telegram.Bot.Types.Enums.ParseMode.Html);

        }

        ScheduleSSTU schedule = new ScheduleSSTU(url, group,fl);
        WrapperAboveData<string> data =  await schedule.LoadData();
        var res = data.GetData();
        Console.WriteLine(String.Join("\n", res));
        return await client.SendTextMessageAsync(chatId, String.Join("\n",res), parseMode: Telegram.Bot.Types.Enums.ParseMode.Html);

        
    }

    public override string[] GetParamsArrStr(string message)
    {
        string[] tex =message.Split(',', StringSplitOptions.RemoveEmptyEntries);
        string[] res =new string[tex.Length-1];
        for(int i = 0; i < tex.Length; i++)
        {
            res[i]=tex[i];
        }
        
        return res;
    }
}
