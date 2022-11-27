using System.Text.RegularExpressions;

namespace AssistantBotAPI.OptionСlasses;

public class IntelligentTextMessaging
{
    string text;

    public IntelligentTextMessaging(string text)
    {
        this.text = text;
    }
    public string GetTextOrStickResponse()
    {
        string? str = null;
        if (Regex.IsMatch(text, @"([Пп]ривет|[Зз]дравствуйте)"))
        {
            str = StandardBot.messageHello;
        }
        else
        {
            if (Regex.IsMatch(text, @"([Хх]орошо|[Оо]тлично)"))
            {
                str = StandardBot.stickerNice;
            }
            else if (Regex.IsMatch(text, @"([Пп]лохо|[Уу]жасно)"))
            {
                str = StandardBot.stickerNotNice;
            }
            else str = StandardBot.messageHelp;
        }
        return str;
    }
}

