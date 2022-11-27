using System.Text.RegularExpressions;
namespace AssistantBotAPI.OptionСlasses.Weather;

public class WrapperWeather : IAsyncGetStringWeatherable
{
    
    private string RegexLocity => "[А-ЩЭ-ЯЁ][а-яё]{1,}(|(-[А-ЩЭ-ЯЁ][а-яё]{1,})|( [А-ЩЭ-ЯЁ][а-яё]{1,}))";
    private string locity;
    public WrapperWeather(string locity)
    {
        //this.locity = locity.Split("r")[1];
        this.locity = locity;
    }
    string Locity { get { return locity; } }
    public async Task<string> getStringWeather(string currenOrWeek = "Текущая")
    {
        if(!Regex.IsMatch(locity,RegexLocity, RegexOptions.ECMAScript))
        {
            return await Task.Run(()=>"Неправильное название города ");
        }
        IAsyncGetStringWeatherable weather = new Weather(Locity);
        return await weather.getStringWeather(currenOrWeek);
    }
}

