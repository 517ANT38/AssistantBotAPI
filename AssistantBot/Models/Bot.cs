using AssistantBotAPI.Models.Commands;
using AssistantBotAPI.OptionСlasses;
using OptionСlasses.Calendar;
using OptionСlasses.Reminder;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Exceptions;
using Telegram.Bot.Polling;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;

namespace AssistantBotAPI.Models
{
    public class Bot
    {
        public TelegramBotClient botClient;
        private List<Command> commandsList;
        
        public Bot(List<Command> commands)
        {


            commandsList = new List<Command>(StandardBot.CommandsList);
            
            if(commands != null)
                  commandsList = commandsList.Concat(commands).ToList();


            botClient = new TelegramBotClient(AppSettings.Token);
            //Console.WriteLine("!!!");
            

        }
        public Bot() : this(null) { }
        
        public List<Command> Commands
        {
            get { return new List<Command>(commandsList); }

        }
        public void addCommand(Command command)
        {
            commandsList.Add(command);
        }
        public void delCommand(string name)
        {
            foreach (var item in StandardBot.CommandsList)
            {
                if (name.Equals(item.Name)) throw new ArgumentException("Базовые команды удалить нельзя!");
            }
            foreach (var item in commandsList)
            {
                if (name.Equals(item.Name))
                {
                    commandsList.Remove(item);
                    break;
                }
            }
        }
        public async Task startBot()
        {
            using var cts = new CancellationTokenSource();
            Console.WriteLine("Run!");
            // StartReceiving does not block the caller thread. Receiving is done on the ThreadPool.
            var receiverOptions = new ReceiverOptions
            {
                AllowedUpdates = Array.Empty<UpdateType>()

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
        }

        private Task HandlePollingErrorAsync(ITelegramBotClient botClient, Exception exception, CancellationToken cancellationToken)
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

        private  async Task HandleUpdateAsync(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
        { 
            switch (update.Type)
            {
                case UpdateType.CallbackQuery:
                    {
                       // Console.WriteLine(update.Message.MessageId);
                        await OnCallbackQuery(update.CallbackQuery, botClient);
                        break;
                    }
                case UpdateType.Message:
                    {
                        var chatId = update.Message.Chat.Id;
                        Message sentMessage = await newMessage(update.Message.Text, chatId, cancellationToken);
                        break;
                    }
                default:
                    break;


            }

                    //if (message.Text is not { } messageText  )
                    //    return;

                   
            
        }

        private  async Task<Message> newMessage(string text,long chatId,CancellationToken cancellationToken)
        {
            Command command=null;
            foreach(var item in commandsList)
            {
                if (item.Contains(text))
                {
                    command = item;
                    break;
                }
                

            }
            //Console.WriteLine(command.Name);
            if(command == null)
            {
                IntelligentTextMessaging intelligent = new IntelligentTextMessaging(text);
                string str = intelligent.GetTextOrStickResponse();
                if (Regex.IsMatch(str, StandardBot.patternUri))
                {
                    return await botClient.SendStickerAsync(
                        chatId: chatId,
                        sticker: str,
                        cancellationToken: cancellationToken);
                }
                else
                    return await botClient.SendTextMessageAsync(
                        chatId: chatId,
                        text: str,
                        cancellationToken: cancellationToken,
                        parseMode:Telegram.Bot.Types.Enums.ParseMode.Html
                    );
            }
            else
            {
                return await command.Execute(chatId, botClient, command.GetParamsArrStr(text));
            }
            
        }
        public async static Task OnCallbackQuery(CallbackQuery query, ITelegramBotClient bot)
        {
            var cbargs = query.Data.Split(' ');
            //Console.WriteLine(cbargs[2]);
            switch (cbargs[0].Trim())
            {
                case "month":
                    {
                        var month = new Month((MonthName)Enum.Parse(typeof(MonthName), cbargs[2]), int.Parse(cbargs[1]));
                        var mkeyboard = Calendar.CreateCalendar(month);

                        await bot.EditMessageReplyMarkupAsync(query.Message.Chat.Id,query.Message.MessageId,mkeyboard,default);
                        break;
                    }
                case "year":
                    var ykeyboard = Calendar.CreateCalendar(int.Parse(cbargs[1]));

                    await bot.EditMessageReplyMarkupAsync(query.InlineMessageId, ykeyboard);
                    break;
                default:

                     await bot.AnswerCallbackQueryAsync(cbargs[1]);
                    break;
            }
        }
    }
    
}
