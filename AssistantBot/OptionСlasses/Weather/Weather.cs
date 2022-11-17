using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Newtonsoft.Json;
using OpenMeteo;

namespace OptionСlasses.Weather;

public class Weather
{
    private OpenMeteo.OpenMeteoClient client = new OpenMeteo.OpenMeteoClient();
    private GeocodingOptions geocodingData;


    public Weather(string locality)
    {
        this.geocodingData = new GeocodingOptions(locality, "ru", 1);
    }
    private async Task<WeatherForecast> getWeather()
    {
        var tmp = await client.GetLocationDataAsync(geocodingData);

        WeatherForecastOptions options = new WeatherForecastOptions();
        options.Hourly = HourlyOptions.All;
        options.Windspeed_Unit = WindspeedUnitType.ms;
        options.Latitude = tmp.Locations[0].Latitude;
        options.Longitude = tmp.Locations[0].Longitude;
        options.Daily = DailyOptions.All;
        options.Timezone= tmp.Locations[0].Timezone;
        return await client.QueryAsync(options);
    }
    private async Task<string> getStringCurrentWeather()
    {
        WeatherForecast forecast = await getWeather();
        if (forecast == null) return "Такого города нет!";
        int index = DateTime.Now.Hour;
        string res = $"Погода в {geocodingData.Name} на текущий момент:\n";
        res += (" Температура: " + forecast.CurrentWeather.Temperature+ "°C;\n");
        res += (" Давление: " + Math.Floor(forecast.Hourly.Surface_pressure[index] /1.33317)+ " мм рт. ст.;\n");
        res +=" Осадки: "+namePrecipitation((int)forecast.Hourly.Weathercode[index])+ ";\n";
        res += (" Количество осадков: " + forecast.Hourly.Precipitation[index] + " мм;\n");
        res += (" Скорость ветра: " + forecast.CurrentWeather.Windspeed)+" м/с;\n";
        res += " Направление ветра: " + getWindDirection(forecast.CurrentWeather.WindDirection) + "\n";
        return res;
    }
    private async Task<string> getStringWeekHourly()
    {
        WeatherForecast forecast = await getWeather();
        if (forecast == null) return "Такого города нет!";
        string res = $"Погода в {geocodingData.Name} на сегодня:\n";
        for(int i = 0; i < 24; i+=3)
        {
            res += " " + DateTime.Parse(forecast.Hourly.Time[i]).ToString("t")+"\n";
            res += (" Температура: " + forecast.Hourly.Temperature_2m[i] + "°C;\n");
            res += (" Давление: " + Math.Floor(forecast.Hourly.Surface_pressure[i] / 1.33317) + " мм рт. ст.;\n");
            res += " Осадки: " + namePrecipitation((int)forecast.Hourly.Weathercode[i]) + ";\n";
            res += (" Количество осадков: " + forecast.Hourly.Precipitation[i] + " мм;\n");
            res += (" Скорость ветра: " + forecast.Hourly.Windspeed_10m[i]) + " м/с;\n";
            res += " Направление ветра: " + getWindDirection(forecast.Hourly.Winddirection_10m[i]) + "\n";
            res += " ------------------------------------------------------- \n";
        }
        return res;
    }
    private async Task<string> getStringWeekWeather()
    {
        WeatherForecast forecast = await getWeather();
        if (forecast == null) return "Такого города нет!";
        string res = $"Погода в {geocodingData.Name} на неделю:\n";
        for (int i = 0; i < 7; i++)
        {
            res += " " + DateTime.Parse(forecast.Daily.Time[i]).ToString("D");
            res+=" "+ getWeekDay(DateTime.Parse(forecast.Daily.Time[i]).DayOfWeek)+"\n";
            float tmp = (forecast.Daily.Apparent_temperature_max[i] + forecast.Daily.Apparent_temperature_min[i]) / 2;
            res +=" Температура: "+tmp + " мм рт. ст.;\n";
            res += " Осадки: " + namePrecipitation((int)forecast.Daily.Weathercode[i]) + "\n";
            res += " Количество осадков: " + forecast.Daily.Precipitation_sum[i]+ " мм;\n";
            res += " Скорость ветра: " + forecast.Daily.Windspeed_10m_max[i] + " м/с;\n";
            res += " Направление ветра: " + getWindDirection(forecast.Daily.Winddirection_10m_dominant[i]) + "\n";
            res += " ------------------------------------------------------- \n";
        }
        //forecast.Daily.Winddirection_10m_dominant;
        return res;
        
    }
    public async Task<string> getStringWeather(string currenOrWeek="Текущая")
    {
        if (Regex.IsMatch(currenOrWeek, "[Тт]екущая"))
            return await getStringCurrentWeather();
        else if (Regex.IsMatch(currenOrWeek, @"[Нн]а неделю"))
            return await getStringWeekWeather();
        else if (Regex.IsMatch(currenOrWeek, "[Нн] сегодня"))
            return await getStringWeekHourly();
        else
            return "Такой погоды нет!";
    }
    private static string namePrecipitation(int a)
    {
        switch (a)
        {
            case 0:
                return "Чистое небо";
            case 1:
                return "В основном ясно";
            case 2:
                return "Частично облачно";
            case 3:
                return "Пасмурно";
            case 45:
            case 48:
               return "Туман";
            case 51:
                return "Моросящий дождь легкая интенсивность";
            case 53:
                return "Моросящий дождь умеренная интенсивность";
            case 55:
                return "Моросящий дождь плотная интенсивность";
            case 56:
                return "Замерзающий моросящий дождь легкая интенсивность";
            case 57:
                return "Замерзающий моросящий дождь плотная интенсивность";
            case 66:
                return "Ледяной дождь легкая интенсивность";
            case 67:
                return "Ледяной дождь сильная интенсивность";
            case 71:
                return "Снегопад незначительная интенсивность";
            case 73:
                return "Снегопад умеренная интенсивность";
            case 75:
                return "Снегопад сильная интенсивность";
            case 77:
                return "Снежные зерна";
            case 80:
                return "Ливень незначительная интенсивность";
            case 81:
                return "Ливень умеренная интенсивность";
            case 82:
                return "Ливень сильная интенсивность";
            case 85:
                return "Снежный ливень незначительная интенсивность";
            case 86:
                return "Снежный ливень сильная интенсивность";
            case 95:
                return "Гроза: незначительная или умеренная";
            case 96:
                return "Гроза с легким  градом";
            case 99:
                return "Гроза с сильным  градом";
            default:
                return "Гроза с легким и сильным градом";
        }
       
    }
    private static string getWeekDay(DayOfWeek dayOfWeek)
    {
        switch (dayOfWeek)
        {
            case DayOfWeek.Monday:
                return "Понедельник";
            case DayOfWeek.Tuesday:
                return "Вторник";
            case DayOfWeek.Wednesday:
                return "Среда";
            case DayOfWeek.Thursday:
                return "Четверг";
            case DayOfWeek.Friday:
                return "Пятница";
            case DayOfWeek.Saturday:
                return "Суббота";
            case DayOfWeek.Sunday:
                return "Воскресенье";
            default:
                return "";
        }
    }
    private static string getWindDirection(float p)
    {
        if (p == 0 || p == 360)
            return "Северный ветер";
        else if (p == 180)
            return "Южный ветер";
        else if (p == 90)
            return "Восточный ветер";
        else if (p == 270)
            return "Западный ветер";
        else if (p > 0 && p < 90)
            return "Северо-Восточный ветер";
        else if (p > 90 && p < 180)
            return "Юго-Восточный ветер";
        else if (p > 180 && p < 270)
            return "Юго-Западный ветер";
        else if (p > 270 && p < 360)
            return "Северо-Западный ветер";
        return "Ветра нет";
    }

}

