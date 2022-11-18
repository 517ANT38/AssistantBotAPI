using Microsoft.Data.Sqlite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace OptionСlasses.Joke;

public class Joke
{
    private string type;
    public Joke(string type = "IT")
    {
        this.type = type;
    }
    private string RegexPattern=> "[A-Za-zА-ЩЭ-ЯЁа-щэё][A-Za-zА-Яа-яЁё]{1,}";
    public string TypeJoke { get { return type; }  }
    private static string GetJokeFromDB(string type)
    {
        string res = null;
        long countCort = 0;
        string sqlCountCrt = "SELECT count(2) from Joke" + $" WHERE Type='{type}'";
        string sqlExpession = "SELECT TextJoke FROM Joke" + $" WHERE Type='{type}';";
        using (var connection = new SqliteConnection(@"Data Source=C:\Users\user\source\repos\AssistantBot\AssistantBot\AssistentData\AssistentBotDataBase.db"))
        {
            connection.Open();
            SqliteCommand sqliteCommand = new SqliteCommand(sqlCountCrt, connection);
            SqliteCommand sqliteCommand1 = new SqliteCommand(sqlExpession, connection);
            using (SqliteDataReader reader = sqliteCommand.ExecuteReader())
            {
                if (reader.HasRows) // если есть данные
                {
                    reader.Read();
                    countCort = (long)reader.GetValue(0);


                }
            }
            using (SqliteDataReader reader = sqliteCommand1.ExecuteReader())
            {
                if (reader.HasRows) // если есть данные
                {

                    Random random = new Random();
                    for (long i = 0; i < random.NextInt64(1, countCort); i++)
                    {
                        reader.Read();
                    }
                    res = "Анекдот: <i>" + (string)reader["TextJoke"] + "</i>";

                }
                else res = "Такого типа шуток нет . Возможно вас расмешит этот анекдот: " +
                        "<i>Гораздо проще просто запустить программу и посмотреть, что пойдет не так, чем разбираться в коде.</i>";
            }
        }
        return res;
    }
    public string GetJoke()
    {
        if (!Regex.IsMatch(type, RegexPattern, RegexOptions.IgnoreCase))
            return "Такого типа шуток нет . Возможно вас расмешит этот анекдот: " +
                        "<i>Гораздо проще просто запустить программу и посмотреть, что пойдет не так, чем разбираться в коде.</i>";
        string res =  null;
        try
        {
            res= GetJokeFromDB(type);
        }
        catch(SqliteException e)
        {
            res = "А ведь программисты предупреждали, что 2020 - это 5 умножить на 404...";
        }
        return res;
    }
}

