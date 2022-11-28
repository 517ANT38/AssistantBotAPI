using System.Text.RegularExpressions;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace AssistantBotAPI.Models.Commands
{
    public abstract class Command
    {
        public virtual string TypeCommand => "not with files";
        public abstract string Name { get; }
        public abstract Task<Message> Execute(long chatId, TelegramBotClient client,params string[] arr);

        public abstract bool Contains(string message);
        public virtual string[] GetParamsArrStr(string message)
        {
            Regex regex = new Regex($"{Name}");
            string[] tex = regex.Split(message)[1].Split(",", StringSplitOptions.RemoveEmptyEntries);
            for (int i = 0; i < tex.Length; i++)
            {
                tex[i] = tex[i].Trim();
            }
            return tex;
        }

    }
}
