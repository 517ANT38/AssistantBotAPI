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
using Telegram.Bot.Types.ReplyMarkups;
using AssistantBotAPI.OptionСlasses.fileProcessing;
using AssistantBotAPI.JobWithData;

namespace AssistantBotAPI.OptionСlasses.Reminder;

public class Reminder
{
    private string remin;
    private long chatID;
    private TimeSpan dateTime;
    public static string BoolAndIdFile { get; } = @"AssistentData\BoolAndIdFile.json";
    public Reminder(string remin, long chId, TimeSpan time)
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
            int count = (int)CountRemind(chatID);
            if (count >= 5)
                throw new ArgumentException("Нельзя создать больше пяти напоминания!");
            try
            {
                string sql = "INSERT INTO Reminder(hashe_sum,num,chat_id,reminder_p) " +
                    $"VALUES ({hashe},{count + 1},{chatID},'{remin}');";
                SqliteCommand sqliteCommand = new SqliteCommand(sql, connection);
                sqliteCommand.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                throw new ArgumentException("Такая напоминалка уже есть");
            }
        }
        ThreadStartRemind(botClient, dateTime, hashe, chatID);
    }
    private static long CountRemind(long chatID)
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

                    return (long)a;


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
    private static async void ThreadStartRemind(ITelegramBotClient botClient, TimeSpan timeSleep, int hashe, long chatID)
    {
        Thread thread = null;
        await specVarAsync(chatID, hashe);

        thread = new Thread(async () =>
        {

            Thread.Sleep(timeSleep);
            string str = remind(hashe);
            deleteReminder(hashe);
            if (thread != null)
            {
                await actionMessageRemin(str, thread, botClient, hashe, chatID);
            }
            //Console.WriteLine("sdklfjsdkfjlskfjlsdjfkl");
        }, 256000);
              
        thread.Start();

    }
    private static void deleteReminder(int hashe)
    {
        using (var connection = new SqliteConnection(@"Data Source=AssistentData\AssistentBotDataBase.db"))
        {
            connection.Open();

            string sql = $"DELETE FROM Reminder WHERE ({hashe}=hashe_sum);";

            SqliteCommand sqliteCommand = new SqliteCommand(sql, connection);
            sqliteCommand.ExecuteNonQuery();
        }
        
    }
    private static async Task<bool> FinishThread(long id, int hashe_sum)
    {
        FileFinishThreadCl f = new FileFinishThreadCl();
        var a=await f.ReadAsync(BoolAndIdFile);
        foreach (var item in a)
        {
            if (item.ChatId == id && item.Hashe == hashe_sum)
            {
                return item.Finish;
            }
        }
        return false;
    }
    private static async Task specVarAsync(long id, int hashe_sum)
    {
        FileFinishThreadCl cl = new FileFinishThreadCl();
        var s = await cl.ReadAsync(BoolAndIdFile);
        if (s == null)
        {
            s = new List<ReminderFinshJob>();
        }
        ReminderFinshJob job = new ReminderFinshJob(id, hashe_sum, false);
        s.Add(job);
        await cl.WriteAsync(BoolAndIdFile, s);
    }
    private static async Task  actionMessageRemin(string str, Thread thread, ITelegramBotClient botClient, int hashe, long chatID)
    {
        //await Task.Delay(new TimeSpan(0, 1, 0));
        Message mes = await botClient.SendTextMessageAsync(chatID, str, Telegram.Bot.Types.Enums.ParseMode.Html, replyMarkup: new InlineKeyboardMarkup(new InlineKeyboardButton[] { InlineKeyboardButton.WithCallbackData("Принято", $"Принято {hashe}") }));
        await Task.Delay(new TimeSpan(0, 1, 0));
        var b = await FinishThread(chatID, hashe);
        if (b)
        {
            // Console.WriteLine("^^^^^^^^^^^^^^");


            await botClient.EditMessageReplyMarkupAsync(chatID, mes.MessageId, null, default);

            return;
        }
        else
        {
            await botClient.EditMessageReplyMarkupAsync(chatID, mes.MessageId, null, default);

            await actionMessageRemin(str, thread, botClient, hashe, chatID);
        }
    }
}