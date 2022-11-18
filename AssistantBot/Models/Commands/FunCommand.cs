using JobWithData;
using Microsoft.Data.Sqlite;
using OptionСlasses.Joke;
using System.Text.RegularExpressions;
using Telegram.Bot;
using Telegram.Bot.Types;
namespace AssistantBotAPI.Models.Commands;

public class FunCommand : Command
{
    public override string Name => @"/FunCommand";

    

    public override bool Contains(string message)
    {
        return message.Contains(Name);
    }

    
    public override async Task<Message> Execute(long chatId, TelegramBotClient client, params string[] arr)
    {
        Joke joke;
        string res = null;
        if(arr.Length > 0)
        {
            joke = new Joke(arr[0]);
            
        }
        joke = new Joke();
        res =joke.GetJoke();
            
        return await client.SendTextMessageAsync(chatId,res, parseMode: Telegram.Bot.Types.Enums.ParseMode.Html);
    }

    

    //public override string[] GetParamsArrStr(string message)
    //{
    //    string[] tex = message.Split(' ', StringSplitOptions.RemoveEmptyEntries);
    //    string[] res = new string[tex.Length - 1];
    //    for (int i = 1, j = 0; i < tex.Length; i++, j++)
    //    {
    //        res[j] = tex[i];
    //    }
    //    return res;
    //}
    
}
