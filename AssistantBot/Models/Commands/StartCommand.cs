using JobWithData;
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
    

    public override Task<Message> Execute(long chatId, TelegramBotClient client, params string[] arr)
    {
        throw new NotImplementedException();
    }

    public override string[] GetParamsArrStr(string message)
    {
        throw new NotImplementedException();
    }
}
