using JobWithData;
using OptionСlasses.Shedule;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace AssistantBotAPI.Models.Commands
{
    public abstract class Command
    {
        public abstract string Name { get; }
        public abstract List<string> RegStringChekData { get; }
        public abstract Task<Message> Execute(long chatId, TelegramBotClient client,params string[] arr);

        public abstract bool Contains(string message);
        public virtual string[] GetParamsArrStr(string message)
        {
            string[] tex = message.Split(' ', StringSplitOptions.RemoveEmptyEntries);
            string[] res = new string[tex.Length - 1];
            for (int i = 1, j = 0; i < tex.Length; i++, j++)
            {
                res[j] = tex[i];
            }
            return res;
        }
        
    }
}
