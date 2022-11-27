using AssistantBotAPI.OptionСlasses.Help;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace AssistantBotAPI.Models.Commands;
internal class HelpCommand : Command
{
    public override string Name => "/help";

    public override bool Contains(string message)
    {
        return message.Contains(Name);
    }    

    public override async Task<Message> Execute(long chatId, TelegramBotClient client, params string[] arr)
    {
        Help help;
        if(arr.Length > 0)
            help = new Help(arr[0]);
        else
            help = new Help();
        string res = help.GetDescriptionOfcommandParad();
        return await client.SendTextMessageAsync(chatId, res, parseMode: Telegram.Bot.Types.Enums.ParseMode.Html);

    }
}
