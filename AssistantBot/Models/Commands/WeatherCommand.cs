using JobWithData;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace AssistantBotAPI.Models.Commands;

internal class WeatherCommand : Command
{
    public override string Name => "/weather";

    public override List<string> RegStringChekData => throw new NotImplementedException();

    public override bool Contains(string message)
    {
        return message.Contains(this.Name);
    }

    public override Task<Message> Execute(long chatId, TelegramBotClient client, params string[] arr)
    {
        throw new NotImplementedException();
    }

   
    
}
