using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.InputFiles;

namespace AssistantBotAPI.Models.Commands;

public class ConverterCommand : Command
{
    public override string TypeCommand => "with files";
    public override string Name => "/converter";

    public override bool Contains(string message)
    {
        return message.Contains(Name);
    }

    public override async Task<Message> Execute(long chatId, TelegramBotClient client, params string[] arr)
    {
        if (arr.Length < 0)
        {
            return await client.SendTextMessageAsync(chatId, "Не переданны расширения файлов(из чего конвертировать, и во что)", parseMode: Telegram.Bot.Types.Enums.ParseMode.Html);
        }
        else if (arr.Length < 2)
        {
            return await client.SendTextMessageAsync(chatId, "Не переданны расширения файлов(во что конвертировать)", parseMode: Telegram.Bot.Types.Enums.ParseMode.Html);

        }
        else
        {
            foreach (var item in StandardBot.Converters)
            {
                var s=arr[0].ToLower()+" "+arr[1].ToLower();
                if(item.ConvertName == s)
                {
                    var fileOut = StandardBot.outFileDirect +"File"+ item.ConvertToType;
                    await item.PerformAsync(StandardBot.destinationFileDirectPath,  fileOut);
                    using (Stream stream = System.IO.File.OpenRead(fileOut))
                    {
                        return await client.SendDocumentAsync(
                            chatId: chatId, 
                            document: new InputOnlineFile(content: stream, fileName: StandardBot.nameResFile+item.ConvertToType),
                            caption: @"Файл конвертирован.");
                    }
                }
            }
            return await client.SendTextMessageAsync(chatId, "Так конвертировать я не могу.", parseMode: Telegram.Bot.Types.Enums.ParseMode.Html);
        }
    }
    
}

