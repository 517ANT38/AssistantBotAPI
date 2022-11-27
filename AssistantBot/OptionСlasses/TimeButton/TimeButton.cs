using Telegram.Bot.Types.ReplyMarkups;

namespace AssistantBotAPI.OptionСlasses.TimeButton;

public static class TimeButton
{
    public static InlineKeyboardMarkup TimeIncDic()
    {
        var keyboard = new InlineKeyboardMarkup(new [] {
                new [] {
                    InlineKeyboardButton.WithCallbackData("+1 час","час"),
                    InlineKeyboardButton.WithCallbackData("+1 мин","мин"),
                    InlineKeyboardButton.WithCallbackData("+1 сек","сек")

                },
                new [] {
                    InlineKeyboardButton.WithCallbackData("-1 час","-час"),
                    InlineKeyboardButton.WithCallbackData("-1 мин","-мин"),
                    InlineKeyboardButton.WithCallbackData("-1 сек","-сек")

                },
                new [] {
                    InlineKeyboardButton.WithCallbackData("Установить дату и время","StartAddTime")
                }
            });
        return keyboard;
    }

}

