using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

using System.Net.Http.Json;
using Newtonsoft.Json;
using Microsoft.Data.Sqlite;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace OptionСlasses.Reminder;

public class Reminder
{
    private string remin;
    private long chatID;
    private TimeSpan dateTime;
    //private Thread thread;
    public Reminder(string remin,long chId,TimeSpan time)
    {
        this.remin = remin;
        this.chatID = chId;
        this.dateTime = time;
    }
    public void CreateReminder(ITelegramBotClient botClient)
    {
        int hashe = HashCodeForBD(remin + chatID.ToString());
        using (var connection = new SqliteConnection(@"Data Source=AssistentData\AssistentBotDataBase.db"))
        {
            connection.Open();
            int count = CountRemind(chatID);
            if (count >= 5)
                throw new ArgumentException("Нельзя создать больше пяти напоминания!");
            string sql = "INSERT INTO Reminder(hashe_sum,num,chat_id,reminder_p) " +
                $"VALUES ({hashe},{count + 1},{chatID},'{remin}'";
            SqliteCommand sqliteCommand = new SqliteCommand(sql, connection);
            sqliteCommand.ExecuteNonQuery();
        }
        ThreadStartRemind(botClient, dateTime, hashe, chatID);
    }
    private static int CountRemind(long chatID)
    {
        using (var connection = new SqliteConnection(@"Data Source=AssistentData\AssistentBotDataBase.db"))
        {
            connection.Open();
            
            string sql = $"SELECT num FROM Reminder WHERE chat_id={chatID} ORDER BY num";
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
    private static string remind(int hashe)
    {
        
        using (var connection = new SqliteConnection(@"Data Source=AssistentData\AssistentBotDataBase.db"))
        {
            connection.Open();

            string sql = $"SELECT reminder_p FROM Reminder WHERE ({hashe}=hashe_sum);";
            
            SqliteCommand sqliteCommand = new SqliteCommand(sql, connection);
            using (SqliteDataReader reader = sqliteCommand.ExecuteReader())
            {
               // Console.WriteLine(DateTime.Now);
                if (reader.Read()) // если есть данные
                {
                    //Console.WriteLine("!!!!!");
                    var a = reader.GetValue(0);
                   // Console.WriteLine(a.ToString());
                    if (a.GetType() == typeof(DBNull))
                    {
                        return null;
                    }
                    else
                    {
                        
                        
                        return (string)a;
                    }
                }
                
                return null;
            }
        }
    }
    private static void ThreadStartRemind(ITelegramBotClient botClient,TimeSpan timeSleep,int hashe,long chatID)
    {
        Thread thread = new Thread(async () =>
        {
            string str = remind(hashe);

            Thread.Sleep(timeSleep);
            Message mes =await botClient.SendTextMessageAsync(chatID, str);
        },256000);
        thread.Start();
    }
}
