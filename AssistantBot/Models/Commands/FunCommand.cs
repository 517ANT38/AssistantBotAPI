using JobWithData;
using Microsoft.Data.Sqlite;
using OptionСlasses.Joke;
using System.Text.RegularExpressions;
using Telegram.Bot;
using Telegram.Bot.Types;
namespace AssistantBotAPI.Models.Commands;

public class FunCommand : Command
{
    public override string Name => @"/funcommand";

    

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
            Console.WriteLine(arr[0]);
        }
        else
            joke = new Joke();
        res =joke.GetJoke();
            
        return await client.SendTextMessageAsync(chatId,res, parseMode: Telegram.Bot.Types.Enums.ParseMode.Html);
    }


    public override string[] GetParamsArrStr(string message)
    {
        Regex regex = new Regex($"{Name}");
        string[] tex = regex.Split(message)[1].Split(" ", StringSplitOptions.RemoveEmptyEntries);

        return tex;
    }

}
