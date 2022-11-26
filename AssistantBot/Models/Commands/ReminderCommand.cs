using JobWithData;
using OptionСlasses.Calendar;
using OptionСlasses.Reminder;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;

namespace AssistantBotAPI.Models.Commands;
public class ReminderCommand : Command
{
    public override string Name => "/reminder";


    public override bool Contains(string message)
    {
        return message.Contains(this.Name);
    }

    
    public override async Task<Message> Execute(long chatId, TelegramBotClient client, params string[] arr)
    {
        if (arr.Length < 0)
        {
            return await client.SendTextMessageAsync(chatId, "Неуказаны аргументы : О чем напоминать, и время (насколько ставить напоминалку).", Telegram.Bot.Types.Enums.ParseMode.Html);

        }
        else if (arr.Length < 2)
        {
            return await client.SendTextMessageAsync(chatId, "Неуказан аргумент : Время (насколько ставить напоминалку).", Telegram.Bot.Types.Enums.ParseMode.Html,replyMarkup:Calendar.CreateCalendar(new Month(MonthName.December,2022)));
        }
        else
        {
            int  d = int.Parse(arr[1]);
            Reminder reminder=new Reminder(arr[0], chatId,new TimeSpan(0));
            return await client.SendTextMessageAsync(chatId, "Неуказан аргумент : Время (насколько ставить напоминалку).", Telegram.Bot.Types.Enums.ParseMode.Html);
        }
    }
    
    //public override string[] GetParamsArrStr(string message)
    //{
    //    throw new NotImplementedException();
    //}
}
