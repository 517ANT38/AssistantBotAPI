using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;

namespace OptionСlasses.Calendar;
public class Calendar
{

    public static InlineKeyboardMarkup CreateCalendar(Month mon)
    {
        var calendar = new InlineKeyboardButton[mon.Weeks + 3][];
        var pos = 0;
        calendar[0] = new InlineKeyboardButton[1]
        {
                InlineKeyboardButton.WithCallbackData($"{mon.Name} {mon.Year}", $"year {mon.Year}")
        };
        var days = new[] { "Mo", "Tu", "We", "Th", "Fr", "Sa", "Su" };
        calendar[1] = new InlineKeyboardButton[7];
        for (int i = 0; i < 7; i++)
        {
            calendar[1][i] = InlineKeyboardButton.WithCallbackData(days[i], $"{((DayOfWeek)i)}");
        }
        for (int i = 2; i < mon.Weeks + 2; i++)
        {
            calendar[i] = new InlineKeyboardButton[7];
            for (int j = 0; j < 7; j++)
            {
                if (pos < mon.Days.Length)
                {
                    if ((int)mon.Days[pos].Name == j)
                    {
                        calendar[i][j] = InlineKeyboardButton.WithCallbackData($"{mon.Days[pos].Number}", $"{mon.Days[pos].Name}, {mon.Name} {mon.Days[pos].Number}");
                        pos++;
                    }
                    else
                    {
                        calendar[i][j] = InlineKeyboardButton.WithCallbackData("*", "Empty day");
                    }
                }
                else
                {
                    calendar[i][j] = InlineKeyboardButton.WithCallbackData("*", "Empty day");
                }
            }
        }
        calendar[calendar.Length - 1] = new InlineKeyboardButton[2];
        var previousmonth = mon.Name == MonthName.January ? MonthName.December : mon.Name - 1;
        var nextmonth = mon.Name == MonthName.December ? MonthName.January : mon.Name + 1;
        var previousyear = previousmonth == MonthName.December ? mon.Year - 1 : mon.Year;
        var nextyear = nextmonth == MonthName.January ? mon.Year + 1 : mon.Year;
        calendar[calendar.Length - 1][0] = InlineKeyboardButton.WithCallbackData($"{previousmonth}", $"month {previousyear} {((int)previousmonth)}");
        calendar[calendar.Length - 1][1] = InlineKeyboardButton.WithCallbackData($"{nextmonth}", $"month {nextyear} {((int)nextmonth)}");
        return new InlineKeyboardMarkup(calendar);
    }
    public static InlineKeyboardMarkup CreateCalendar(int year)
    {
        var keyboard = new InlineKeyboardButton[6][];
        keyboard[0] = new InlineKeyboardButton[1]{
                InlineKeyboardButton.WithCallbackData($"{year}", $"Year {year}")
            };
        for (int i = 1, n = 0; i < 5; i++)
        {
            keyboard[i] = new InlineKeyboardButton[3];
            for (int j = 0; j < 3; j++, n++)
            {
                var month = (MonthName)n;
                keyboard[i][j] = InlineKeyboardButton.WithCallbackData($"{month}" + $"month {year} {n}");

            }
        }
        keyboard[5] = new InlineKeyboardButton[2]{
                InlineKeyboardButton.WithCallbackData($"{year - 1}",$"year {year - 1}"),
                InlineKeyboardButton.WithCallbackData($"{year + 1}",$"year {year + 1}")
            };
        return new InlineKeyboardMarkup(keyboard);
    }
   
}

