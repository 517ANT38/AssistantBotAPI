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
        
        if (arr.Length <= 0)
        {
            return await client.SendTextMessageAsync(chatId, "Хорошо, укажите на какой день вам надо поставить напоминалку", Telegram.Bot.Types.Enums.ParseMode.Html, replyMarkup: Calendar.CreateCalendar(new Month((MonthName)DateTime.Now.Month, DateTime.Now.Year)));

        }
        else if (arr.Length <= 1)
        {
            return await client.SendTextMessageAsync(chatId, "Хорошо, укажите на какой день вам надо поставить напоминалку", Telegram.Bot.Types.Enums.ParseMode.Html, replyMarkup: Calendar.CreateCalendar(new Month((MonthName)DateTime.Now.Month, DateTime.Now.Year)));
        }
        else
        {
            var d = DateTime.Parse(arr[1]);
            if (DateTime.Now >= d)
            {
                return await client.SendTextMessageAsync(chatId, "Некоректные данные о времени.Время истекло.", Telegram.Bot.Types.Enums.ParseMode.Html);
            }

            TimeSpan timeSpan = d - DateTime.Now;
            Reminder reminder=new Reminder(arr[0], chatId,timeSpan);
            reminder.CreateReminder(client);
            return await client.SendTextMessageAsync(chatId, "Напоминалка создана.", Telegram.Bot.Types.Enums.ParseMode.Html);
        }
    }

    public override string[] GetParamsArrStr(string message)
    {
        string[] arr = message.Replace(Name, "").Split(',');
        for (int i = 0;i< arr.Length; i++)
        {
            arr[i]=arr[i].Trim();
        }
        return arr;
    }
}
