//using Telegram.Bot;
//using Telegram.Bot.Types;

using AssistantBotAPI.OptionСlasses.Shedule;
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

        IAsyncLoaDatable wrapShedSetGet=new WrapShedSetGetDbClass(chatId, arr);
        var res = await wrapShedSetGet.LoadData();




        return await client.SendTextMessageAsync(chatId, res.Item2, parseMode: Telegram.Bot.Types.Enums.ParseMode.Html);

        
    }

   
}
