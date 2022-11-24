using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

using System.Net.Http.Json;
using Newtonsoft.Json;
using Microsoft.Data.Sqlite;

namespace OptionСlasses.Reminder;

public class Reminder
{
    private string remin;
    private long chatID;
    private DateTime dateTime;
    public Reminder(string remin,long rem,DateTime dateTime)
    {
        this.remin = remin;
        this.chatID = rem;
        this.dateTime = dateTime;
    }
    public void CreateReminder()
    {
        using (var connection = new SqliteConnection(@"Data Source=AssistentData\AssistentBotDataBase.db"))
        {
            connection.Open();
            int count = CountRemind(chatID);
            string sql = "INSERT INTO Reminder(cashe_sum,num,chat_id,reminder_p,date_ipoln) " +
                $"VALUES ({HashCodeForBD(remin + chatID.ToString())},{count + 1},'{remin}','{dateTime}'";
            SqliteCommand sqliteCommand = new SqliteCommand(sql, connection);
            sqliteCommand.ExecuteNonQuery();
        }
    }
    private static int CountRemind(long chatID)
    {
        using (var connection = new SqliteConnection(@"Data Source=AssistentData\AssistentBotDataBase.db"))
        {
            connection.Open();
            
            string sql = $"SELECT num FROM Reminder WHERE chat_id={chatID}";
            SqliteCommand sqliteCommand = new SqliteCommand(sql, connection);
            using (SqliteDataReader reader = sqliteCommand.ExecuteReader())
            {
                if (reader.Read()) // если есть данные
                {

                    var a = reader.GetValue(0);
                    if (a.GetType() == typeof(DBNull))
                    {
                        return 0;
                    }

                    return (int)a;


                }
                else
                {
                    return 0;
                }
            }
        }
    }
    private static int HashCodeForBD(string value)
    {
        int num = 5381;
        int num2 = num;
        for (int i = 0; i < value.Length; i += 2)
        {
            num = (((num << 5) + num) ^ value[i]);

            if (i + 1 < value.Length)
                num2 = (((num2 << 5) + num2) ^ value[i + 1]);
        }
        return num + num2 * 1566083941;
    }
    public static (bool,string?) remind(long chatId)
    {
        using (var connection = new SqliteConnection(@"Data Source=AssistentData\AssistentBotDataBase.db"))
        {
            connection.Open();

            string sql = "SELECT reminder_p FROM Reminder WHERE (chat_id={chatID} and (datetime('now')<=date_ipoln))";
            SqliteCommand sqliteCommand = new SqliteCommand(sql, connection);
            using (SqliteDataReader reader = sqliteCommand.ExecuteReader())
            {
                while(reader.Read()) // если есть данные
                {

                    var a = reader.GetValue(0);
                    if (a.GetType() == typeof(DBNull))
                    {
                        continue;
                    }
                    else
                        return (true,(string)a);


                }
               // reader.Close();
               return (false,null);
            }
        }
    }
}
