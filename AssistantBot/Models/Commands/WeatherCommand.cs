using JobWithData;
using OptionСlasses.Weather;
using System.Text.RegularExpressions;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace AssistantBotAPI.Models.Commands;

internal class WeatherCommand : Command
{
    public override string Name => "/weather";


    public override bool Contains(string message)
    {
        return message.Contains(this.Name);
    }

    public override async Task<Message> Execute(long chatId, TelegramBotClient client, params string[] arr)
    {
        IAsyncGetStringWeatherable weatherable=null;
        string res="Вы не задали параметры: город, и какой вам нужен прогноз(по часовой на текущую дату, текущий, на неделю).";
        if (arr.Length > 0)
        {
            weatherable=new WrapperWeather(arr[0]);
            if (arr.Length > 1)
                res = await weatherable.getStringWeather(arr[1]);
            else
                res =await weatherable.getStringWeather();
        }
        return await client.SendTextMessageAsync(chatId, res, parseMode: Telegram.Bot.Types.Enums.ParseMode.Html);

    }
    //public override string[] GetParamsArrStr(string message)
    //{
    //    Regex regex = new Regex($"{Name}");
    //    string[] tex = regex.Split(message)[1].Split(",", StringSplitOptions.RemoveEmptyEntries);
    //    for (int i = 0; i < tex.Length; i++)
    //    {
    //        tex[i]=tex[i].Trim();
    //    }
    //    return tex;

        
    //}


}
