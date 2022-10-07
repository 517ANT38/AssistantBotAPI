using System.Text.RegularExpressions;
using Telegram.Bot;
using Telegram.Bot.Exceptions;
using Telegram.Bot.Polling;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;

namespace AssistantBotAPI
{
    public class AssistantBot
    {
        static async Task Main(String [] args)
        {
            

            var botClient = new TelegramBotClient("5478947972:AAHRlNd3QlcV5iJFpvGvaezn-i3EMUXfBuY");
            using var cts = new CancellationTokenSource();
            
            // StartReceiving does not block the caller thread. Receiving is done on the ThreadPool.
            var receiverOptions = new ReceiverOptions
            {
                AllowedUpdates = Array.Empty<UpdateType>() // receive all update types
            };
            botClient.StartReceiving(
                updateHandler: HandleUpdateAsync,
                pollingErrorHandler: HandlePollingErrorAsync,
                receiverOptions: receiverOptions,
                cancellationToken: cts.Token
            );

            var me = await botClient.GetMeAsync();

            Console.WriteLine($"Start listening for @{me.Username}");
            Console.ReadLine();
            
            // Send cancellation request to stop bot
            cts.Cancel();
        }
        private static async Task HandleUpdateAsync(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
        {
            // Only process Message updates: https://core.telegram.org/bots/api#message
            bool flag=false;
            //bool dl = false;
            //bool fl = false;
            if (update.Message is not { } message)
                return;
            // Only process text messages
            if (message.Text is not { } messageText)
                return;

            var chatId = message.Chat.Id;
            Message sentMessage;
            //Console.WriteLine($"Received a '{messageText}' message in chat {chatId}.");
            if (string.Compare(messageText, "Привет") == 0
                || string.Compare(messageText, "Здравствуй") == 0
                || messageText=="здравствуй"
                || messageText == "привет")
            {

                sentMessage=await botClient.SendTextMessageAsync(
                chatId: chatId,
                text: StandardMessageBot.messageHello,
                cancellationToken: cancellationToken);
                sentMessage = await botClient.SendTextMessageAsync(
                chatId: chatId,
                text: StandardMessageBot.messageHowYou,
                cancellationToken: cancellationToken);
                flag=true;
             
            }
            else
            {
                if (messageText == "Отлично" || messageText == "Хорошо"
                    || messageText == "хорошо" || messageText == "отлично")
                {
                    if (flag)
                    {
                        sentMessage = await botClient.SendStickerAsync(
                        chatId: chatId,
                        sticker: StandardMessageBot.stickerNice,
                        cancellationToken: cancellationToken);
                        flag = false;
                    }
                }
                else if (string.Compare(messageText, "Плохо") == 0
                    || string.Compare(messageText, "Ужасно") == 0
                    || string.Compare(messageText, "Неочень") == 0
                    || string.Compare(messageText, "плохо") == 0)
                {
                    if (flag)
                    {
                        sentMessage = await botClient.SendStickerAsync(
                        chatId: chatId,
                        sticker: StandardMessageBot.stickerNotNice,
                        cancellationToken: cancellationToken);
                        flag = false;
                    }
                }
                else if (messageText == "Показать расписание")
                {
                   
                    
                    //Спрашиваем у человека какая у него группа
                    //sentMessage = await botClient.SendTextMessageAsync(
                    //        chatId: chatId,
                    //        text: StandardMessageBot.messQuestsGroup,
                    //        cancellationToken: cancellationToken);
                    //Проверяем сообщение с группой на валидность
                    //if (Regex.IsMatch(messageText, StandardMessageBot.pattrenGroup, RegexOptions.IgnoreCase))
                    //{


                       
                        //fl = true;
                        // dl= true; 
                        KeyboardButton button = new KeyboardButton("");
                        sentMessage = await botClient.SendTextMessageAsync(
                            chatId: chatId,
                            text: StandardMessageBot.messQuestSched,
                            replyMarkup: GetShedulButtons(button),
                            cancellationToken: cancellationToken);
                    //}
                    //if (dl) { 
                        var amm = new Schedule("https://rasp.sstu.ru", "б1-ИФСТ-21",messageText);
                        amm.LoadData();
                    //}
                    
                }
                else 
                {
                    KeyboardButton button = new KeyboardButton("");

                    sentMessage = await botClient.SendTextMessageAsync(
                    chatId: chatId,
                    text: StandardMessageBot.messageHelp,
                    cancellationToken: cancellationToken,
                    replyMarkup: GetFunButtons(button)

                    );
                }
                
            }
            // Echo received message text
            
        }

        private static IReplyMarkup GetFunButtons(KeyboardButton button)
        {
            var rtx=new ReplyKeyboardMarkup(button);
            rtx.Keyboard = new List<List<KeyboardButton>>
            {
                new List<KeyboardButton>()
                {
                    new KeyboardButton("Играть"),
                    new KeyboardButton("Показать расписание")
                }

            };
            return rtx;
        }
        private static IReplyMarkup GetShedulButtons(KeyboardButton button)
        {
            var rtx = new ReplyKeyboardMarkup(button);
            rtx.Keyboard = new List<List<KeyboardButton>>
            {
                new List<KeyboardButton>()
                {
                    new KeyboardButton("Понедельник"),
                    new KeyboardButton("Вторник"),
                    new KeyboardButton("Среда"),
                },
                new List<KeyboardButton>()
                {
                    new KeyboardButton("Четверг"),
                    new KeyboardButton("Пятница"),
                    new KeyboardButton("Суббота"),
                },
                new List<KeyboardButton>()
                {
                    new KeyboardButton("Воскресение"),
                    new KeyboardButton("Вся неделя"),
                    
                }

            };
            return rtx;
        }

        private static Task HandlePollingErrorAsync(ITelegramBotClient botClient, Exception exception, CancellationToken cancellationToken)
        {
            var ErrorMessage = exception switch
            {
                ApiRequestException apiRequestException
                    => $"Telegram API Error:\n[{apiRequestException.ErrorCode}]\n{apiRequestException.Message}",
                _ => exception.ToString()
            };

            Console.WriteLine(ErrorMessage);
            return Task.CompletedTask;
        }
        private static async Task<bool> IsMath(string exs,string pat)
        {
            if(Regex.IsMatch(exs, pat, RegexOptions.IgnoreCase))
            {
                return true;
            }
            return false;
        }
    }

}