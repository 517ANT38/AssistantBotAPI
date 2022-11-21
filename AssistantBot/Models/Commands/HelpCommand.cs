using Telegram.Bot;
using Telegram.Bot.Types;

namespace AssistantBotAPI.Models.Commands;
internal class HelpCommand : Command
{
    public override string Name => "/Help";

    public override bool Contains(string message)
    {
        return message.Contains(Name);
    }    

    public override Task<Message> Execute(long chatId, TelegramBotClient client, params string[] arr)
    {
        throw new NotImplementedException();
    }
}
