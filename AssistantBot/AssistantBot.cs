using AssistantBotAPI.Models;
using OptionСlasses.Weather;
using System.Text.RegularExpressions;
using Telegram.Bot;
using Telegram.Bot.Exceptions;
using Telegram.Bot.Polling;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Microsoft.Data.Sqlite;
using File = System.IO.File;
using OpenMeteo;

namespace AssistantBotAPI
{
    public class AssistantBot
    {
        

        static async Task Main(String[] args)
        {
            Bot bot = new Bot();
            await bot.startBot();
            // RunAsync().GetAwaiter().GetResult();
            //Weather weather = new Weather("Саратов");
            //var s = await weather.getStringCurrentWeather();
            //Console.WriteLine(s);
            //  Console.WriteLine(0.9090909090000000001 == 0.9090909090000000001);
            //Weather weather = new Weather("Саратов", true);
            //await weather.mymethod();
            //List<string> list = null;
            //if (File.Exists(@"C:\Users\user\OneDrive\Рабочий стол\jokes1.txt"))
            //{
            //    list = new List<string>(File.ReadAllLines(@"C:\Users\user\OneDrive\Рабочий стол\jokes1.txt"));
            //}
            //string command = "SELECT count(2) from Joke";
            ////for (int i = 0; i < list.Count; i++)
            ////{
            ////    command += $"('Разные','{list[i]}')";
            ////    if (i < list.Count - 1) command += ",";
            ////}
            //using (var connection = new SqliteConnection(@"Data Source=C:\Users\user\source\repos\AssistantBot\AssistantBot\AssistentData\AssistentBotDataBase.db"))
            //{
            //    connection.Open();
            //    SqliteCommand sqliteCommand = new SqliteCommand(command, connection);

            //    using (SqliteDataReader reader = sqliteCommand.ExecuteReader())
            //    {
            //        if (reader.HasRows) // если есть данные
            //        {

            //            var i=(long)reader.GetValue(0);


            //        }
            //    }
            //}

        }
        static async Task RunAsync()
        {
            //    OpenMeteo.OpenMeteoClient client = new OpenMeteo.OpenMeteoClient();

            //    //// Create new geocodingOptions object for Tokyo
            //    GeocodingOptions geocodingData = new GeocodingOptions("Саратов", "ru", 1);

            //    var apiResponse = await client.GetLocationDataAsync(geocodingData);
            //    //var cityData = apiResponse.Locations[0];

            //    //Console.WriteLine(cityData.Name + " is a city in " + cityData.Country + " with a population of " + cityData.Population + " people.");
            //    //Console.WriteLine(cityData.Name + " coordinates are: " + cityData.Latitude + "/" + cityData.Longitude);
            //    //OpenMeteo.OpenMeteoClient client = new OpenMeteo.OpenMeteoClient();

            //    // Make a new api call to get the current weather in tokyo
            //    WeatherForecast weatherData = await client.QueryAsync(geocodingData);

            //    // Output the current weather to console
            //    Console.WriteLine("Weather : " + weatherData.CurrentWeather.Temperature + "°C "+weatherData.CurrentWeather.Windspeed+" "+weatherData.Hourly.Precipitation[0]);
            OpenMeteo.OpenMeteoClient client = new OpenMeteo.OpenMeteoClient();

            // Set custom options
            WeatherForecastOptions options = new WeatherForecastOptions();
            options.Hourly = HourlyOptions.All;
            options.Latitude = 35.6895f;
            options.Longitude = 139.69171f; // For Tokyo

            // Make a new api call to get the current weather in tokyo
            WeatherForecast weatherData = await client.QueryAsync(options);

            // Output the current weather to console
            Console.WriteLine("Weather in Tokyo: " + weatherData.Hourly.Vapor_pressure_deficit[0] + "");
        }
    }
}