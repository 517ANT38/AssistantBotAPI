namespace AssistantBotAPI.OptionСlasses.Weather;

public interface IAsyncGetStringWeatherable
{
    Task<string> getStringWeather(string currenOrWeek = "Текущая");
}

