using JobWithData;
using OptionСlasses.Help;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace AssistantBotAPI.Models.Commands;
public class StartCommand : Command
{
    public override string Name => @"/start";


    public override bool Contains(string message)
    {
        return message.Contains(this.Name);
    }
    

    public async override Task<Message> Execute(long chatId, TelegramBotClient client, params string[] arr)
    {
        string res = "<b>Вас приветствует Ассистент-Бот, что я могу:</b>" + Environment.NewLine;
        Help help = new Help();
        res += help.GetDescriptionOfcommandParad();
        return await client.SendTextMessageAsync(chatId, res, parseMode: Telegram.Bot.Types.Enums.ParseMode.Html);

    }


}
