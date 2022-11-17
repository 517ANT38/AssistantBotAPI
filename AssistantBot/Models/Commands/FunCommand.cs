using Microsoft.Data.Sqlite;
using System.Text.RegularExpressions;
using Telegram.Bot;
using Telegram.Bot.Types;
namespace AssistantBotAPI.Models.Commands;

public class FunCommand : Command
{
    public override string Name => @"/FunCommand";

    public override List<string> RegStringChekData
    {
        get { return new List<string>() { "[A-Za-zА-ЩЭ-ЯЁа-щэё][A-Za-zА-Яа-яЁё]{1,}" }; }
    }

    public override bool Contains(string message)
    {
        return message.Contains(Name);
    }

    
    public override async Task<Message> Execute(long chatId, TelegramBotClient client, params string[] arr)
    {
        string res = "";
        if (arr.Length!=0&&Regex.IsMatch(arr[0], RegStringChekData[0], RegexOptions.IgnoreCase))
            res = GetJokeFromDB(arr[0]);
        else res=GetJokeFromDB();
        return await client.SendTextMessageAsync(chatId,res, parseMode: Telegram.Bot.Types.Enums.ParseMode.Html);
    }

    

    public override string[] GetParamsArrStr(string message)
    {
        string[] tex = message.Split(' ', StringSplitOptions.RemoveEmptyEntries);
        string[] res = new string[tex.Length - 1];
        for (int i = 1, j = 0; i < tex.Length; i++, j++)
        {
            res[j] = tex[i];
        }
        return res;
    }
    private static string GetJokeFromDB(string type="IT")
    {
        long countCort = 0;
        string res = "А ведь программисты предупреждали, что 2020 - это 5 умножить на 404...";
        string sqlCountCrt = "SELECT count(2) from Joke" + $" WHERE Type='{type}'";
        string sqlExpession = "SELECT TextJoke FROM Joke" +$" WHERE Type='{type}';";
        using (var connection = new SqliteConnection(@"Data Source=C:\Users\user\source\repos\AssistantBot\AssistantBot\AssistentData\AssistentBotDataBase.db"))
        {
            connection.Open();
            SqliteCommand sqliteCommand = new SqliteCommand(sqlCountCrt, connection);
            SqliteCommand sqliteCommand1= new SqliteCommand(sqlExpession, connection);
            using (SqliteDataReader reader = sqliteCommand.ExecuteReader())
            {
                if (reader.HasRows) // если есть данные
                {
                    reader.Read();
                    countCort = (long)reader.GetValue(0);


                }
            }
            using(SqliteDataReader reader = sqliteCommand1.ExecuteReader())
            {
                if (reader.HasRows) // если есть данные
                {

                    Random random = new Random();
                    for (long i = 0; i < random.NextInt64(1, countCort); i++)
                    {
                        reader.Read();
                    }
                    res = "Анекдот: <i>" + (string) reader["TextJoke"]+"</i>";

                }
                else res = "Такого типа шуток нет . Возможно вас расмешит этот анекдот: " +
                        "<i>Гораздо проще просто запустить программу и посмотреть, что пойдет не так, чем разбираться в коде.</i>";
            }
        }
        return res;
    }
}
