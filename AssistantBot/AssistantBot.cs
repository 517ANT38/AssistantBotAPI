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
using System.Security.Cryptography;
using System.Text;

using OptionСlasses.Reminder;

namespace AssistantBotAPI
{
    public class AssistantBot
    {


        static async Task Main(String[] args)
        {
            //{
            Bot bot = new Bot();
            await bot.startBot();
            //Translator translator = new Translator("Hello world");
            //Console.WriteLine(translator.GetTranslation());
            //  var a = new GoogleTranslate("auto", "germany");
            //var h = new HttpClient();
            //var a = await h.GetAsync("https://translate.google.ru/?sl=ru&tl=en&text=Привет&op=translate");
            //var b=await a.Content.ReadAsStringAsync();
            //Console.WriteLine(b);

            //    byte[] hashValue;

            //    string messageString = "This is the original message!";

            //    //Create a new instance of the UnicodeEncoding class to
            //    //convert the string into an array of Unicode bytes.
            //    UnicodeEncoding ue = new UnicodeEncoding();

            //    //Convert the string into an array of bytes.
            //    byte[] messageBytes = ue.GetBytes(messageString);

            //    //Create a new instance of the SHA256 class to create
            //    //the hash value.
            //    SHA256 shHash = SHA256.Create();

            //    //Create the hash value from the array of bytes.
            //    hashValue = shHash.ComputeHash(messageBytes);

            //    //Display the hash value to the console.
            //    foreach (byte b in hashValue)
            //    {
            //        Console.Write( b);
            //    }
        }

        //static async Task RunAsync()
        //{
        //    //    OpenMeteo.OpenMeteoClient client = new OpenMeteo.OpenMeteoClient();

        //    //    //// Create new geocodingOptions object for Tokyo
        //    //    GeocodingOptions geocodingData = new GeocodingOptions("Саратов", "ru", 1);

        //    //    var apiResponse = await client.GetLocationDataAsync(geocodingData);
        //    //    //var cityData = apiResponse.Locations[0];

        //    //    //Console.WriteLine(cityData.Name + " is a city in " + cityData.Country + " with a population of " + cityData.Population + " people.");
        //    //    //Console.WriteLine(cityData.Name + " coordinates are: " + cityData.Latitude + "/" + cityData.Longitude);
        //    //    //OpenMeteo.OpenMeteoClient client = new OpenMeteo.OpenMeteoClient();

        //    //    // Make a new api call to get the current weather in tokyo
        //    //    WeatherForecast weatherData = await client.QueryAsync(geocodingData);

        //    //    // Output the current weather to console
        //    //    Console.WriteLine("Weather : " + weatherData.CurrentWeather.Temperature + "°C "+weatherData.CurrentWeather.Windspeed+" "+weatherData.Hourly.Precipitation[0]);
        //    OpenMeteo.OpenMeteoClient client = new OpenMeteo.OpenMeteoClient();

        //    // Set custom options
        //    WeatherForecastOptions options = new WeatherForecastOptions();
        //    options.Hourly = HourlyOptions.All;
        //    options.Latitude = 35.6895f;
        //    options.Longitude = 139.69171f; // For Tokyo

        //    // Make a new api call to get the current weather in tokyo
        //    WeatherForecast weatherData = await client.QueryAsync(options);

        //    // Output the current weather to console
        //    Console.WriteLine("Weather in Tokyo: " + weatherData.Hourly.Vapor_pressure_deficit[0] + "");
        //}

    }
}
