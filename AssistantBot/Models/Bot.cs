using AssistantBotAPI.JobWithData;
using AssistantBotAPI.Models.Commands;
using AssistantBotAPI.OptionСlasses;
using AssistantBotAPI.OptionСlasses.Calendar;
using AssistantBotAPI.OptionСlasses.fileProcessing;
using AssistantBotAPI.OptionСlasses.TimeButton;
using System.Text.RegularExpressions;
using Telegram.Bot;
using Telegram.Bot.Exceptions;
using Telegram.Bot.Polling;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace AssistantBotAPI.Models
{
    public class Bot
    {
        public TelegramBotClient botClient;
        private List<Command> commandsList;

        public Bot(List<Command> commands)
        {


            commandsList = new List<Command>(StandardBot.CommandsList);

            if (commands != null)
                commandsList = commandsList.Concat(commands).ToList();


            botClient = new TelegramBotClient(AppSettings.Token);
            //Console.WriteLine("!!!");


        }
        private static class AppSettings
        {
            public static string Token { get; } = "5650239236:AAGzw60Sdi9GhKbztBG1wGzn5Mkh4A221j8";
            public static string Name { get; } = "AssistantBotAPI";
            public static string interFileBot { get; } = @"AssistentData\InterBot.json";

           // public static Dictionary<SaveCommandAndDat, string> settings =new Dictionary<SaveCommandAndDat, string>();
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

        private async Task HandleUpdateAsync(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
        {
            switch (update.Type)
            {
                case UpdateType.CallbackQuery:
                    {
                        // Console.WriteLine(update.Message.MessageId);
                        await OnCallbackQueryCalender(update.CallbackQuery, botClient);
                        await OnCallbackQueryTime(update.CallbackQuery, botClient);
                        break;
                    }
                case UpdateType.Message:
                    {
                        var chatId = update.Message.Chat.Id;
                        
                        FileProcessing file = new FileProcessing();
                        var t=await file.ReadAsync(AppSettings.interFileBot);
                        //Console.WriteLine(t[0]);
                        if(t != null)
                        {
                            for(int i=0; i<t.Count;i++)
                            {
                                if(t[i].chatID == chatId)
                                {
        
                                    if (t[i].Name == StandardBot.CommandsList[4].Name)
                                    {
                                       // commandName = item.Name;
                                        var s=t[i].Name+" "+ update.Message.Text+","+t[i].Dat;
                                        Message sendMessageA = await newMessage(s, chatId, cancellationToken);
                                        t.Remove(t[i]);
                                        await file.WriteAsync(AppSettings.interFileBot, t);
                                        return;
                                    }
                                }
                            }
                        }
                        Message sendMessage = await newMessage(update.Message.Text, chatId, cancellationToken);
                        break;
                    }
                default:
                    break;

            }

            //if (message.Text is not { } messageText  )
            //    return;



        }

        private async Task<Message> newMessage(string text, long chatId, CancellationToken cancellationToken)
        {
            Command command = null;
            foreach (var item in commandsList)
            {
                if (item.Contains(text))
                {
                    command = item;
                    break;
                }


            }
            
            //Console.WriteLine(command.Name);
            if (command == null)
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
                        parseMode: Telegram.Bot.Types.Enums.ParseMode.Html
                    );
            }
            else
            {
                return await command.Execute(chatId, botClient, command.GetParamsArrStr(text));
            }

        }
        private async static Task OnCallbackQueryCalender(CallbackQuery query, ITelegramBotClient bot)
        {
            var cbargs = query.Data.Split(' ');
            try
            {
                switch (cbargs[0].Trim())
                {
                    case "month":
                        {
                            var month = new Month((MonthName)Enum.Parse(typeof(MonthName), cbargs[2]), int.Parse(cbargs[1]));
                            var mkeyboard = Calendar.CreateCalendar(month);

                            await bot.EditMessageReplyMarkupAsync(query.Message.Chat.Id, query.Message.MessageId, mkeyboard, default);
                            break;
                        }
                    case "year":
                        {
                            var ykeyboard = Calendar.CreateCalendar(int.Parse(cbargs[1]));

                            await bot.EditMessageReplyMarkupAsync(query.InlineMessageId, ykeyboard);
                            break;
                        }
                    case "Monday":
                    case "Tuesday":
                    case "Wednesday":
                    case "Thursday":
                    case "Friday":
                    case "Saturday":
                    case "Sunday":
                        {
                            //Console.WriteLine(cbargs[1]+ cbargs[2] + cbargs[3]);
                            var k = int.Parse(cbargs[2]);
                            var tmp = (k >= 10 && k <= 99) ? cbargs[2] : (0 + cbargs[2]);
                            tmp += ".";
                            int x = Month.ConverterInMonthName(cbargs[1]);
                            tmp += (x >= 10 && x <= 99) ? x : ("0" + x);
                            tmp += ".";
                            tmp += cbargs[3];
                            Console.WriteLine(tmp);
                            var g = DateTime.Parse(tmp+ " "+ DateTime.Now.ToString("t"));
                            await bot.SendTextMessageAsync(query.Message.Chat.Id, $"<b>Назначте время.Установить напоминалку на дату и время:</b> ({g})", Telegram.Bot.Types.Enums.ParseMode.Html, replyMarkup: TimeButton.TimeIncDic());
                            await bot.EditMessageReplyMarkupAsync(query.Message.Chat.Id, query.Message.MessageId, null, default);
                            break;
                        }

                    default:
                        break;

                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
        private async static Task OnCallbackQueryTime(CallbackQuery query, ITelegramBotClient bot)
        {
            
            //Console.WriteLine(cbargs);
            var cbargs = query.Data.Split(' ');
            Func<string, TimeSpan, string> StringСonversion = (str, t) =>
              {
                  var a = query.Message.Text.Split('(',')');
                  var tmp = a[1].Trim().Split(' ');
                  var tmp1 = tmp[1].Trim();
                  Console.WriteLine(a[1].Trim());
                  var x = TimeOnly.Parse(tmp1).Add(t);
                  return a[0]  +"("+ tmp[0].Trim() +" "+ x.ToString("T")+")";

              };
            try
            {
                switch (cbargs[0].Trim())
                {
                    case "час":
                        {
                            var str = query.Message.Text;
                            var res = StringСonversion(str, new TimeSpan(1, 0, 0));
                            Console.WriteLine(StringСonversion(str, new TimeSpan(1, 0, 0)));
                            await bot.EditMessageTextAsync(query.Message.Chat.Id, query.Message.MessageId, res, null, replyMarkup: TimeButton.TimeIncDic());
                            break;
                        }
                    case "мин":
                        {
                            var str = query.Message.Text;
                            var res = StringСonversion(str, new TimeSpan(0, 1, 0));
                            await bot.EditMessageTextAsync(query.Message.Chat.Id, query.Message.MessageId, res, null, replyMarkup: TimeButton.TimeIncDic());
                            break;
                        }
                    case "сек":
                        {
                            var str = query.Message.Text;
                            var res = StringСonversion(str, new TimeSpan(0, 0, 1));
                            await bot.EditMessageTextAsync(query.Message.Chat.Id, query.Message.MessageId, res, null, replyMarkup: TimeButton.TimeIncDic());
                            break;
                        }
                    case "-час":
                        {
                            var str = query.Message.Text;
                            var res = StringСonversion(str, new TimeSpan(-1, 0, 0));
                            await bot.EditMessageTextAsync(query.Message.Chat.Id, query.Message.MessageId, res, null, replyMarkup: TimeButton.TimeIncDic());
                            break;
                        }
                    case "-мин":
                        {
                            var str = query.Message.Text;
                            var res = StringСonversion(str, new TimeSpan(0, -1, 0));
                            await bot.EditMessageTextAsync(query.Message.Chat.Id, query.Message.MessageId, res, null, replyMarkup: TimeButton.TimeIncDic());
                            break;
                        }
                    case "-сек":
                        {
                            var str = query.Message.Text;
                            var res = StringСonversion(str, new TimeSpan(0, 0, -1));
                            await bot.EditMessageTextAsync(query.Message.Chat.Id, query.Message.MessageId, res, null, replyMarkup: TimeButton.TimeIncDic());
                            break;
                        }
                    case "StartAddTime":
                        {
                            var tmp = query.Message.Text.Split('(', ')')[1].Trim();
                            FileProcessing file=new FileProcessing();
                            file.perform(AppSettings.interFileBot, AppSettings.interFileBot, new SaveCommandAndDat(StandardBot.CommandsList[4].Name, query.Message.Chat.Id, tmp));
                            await bot.SendTextMessageAsync(query.Message.Chat.Id, $"<b>Время установлено: </b>{DateTime.Parse(tmp)}. <b>О чем  вам надо напомнить?</b> ", Telegram.Bot.Types.Enums.ParseMode.Html);
                            await bot.EditMessageReplyMarkupAsync(query.Message.Chat.Id, query.Message.MessageId, null, default);
                            break;
                        }
                    default:
                        break;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

    }
}