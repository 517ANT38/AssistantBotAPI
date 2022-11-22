using JobWithData;
using Microsoft.Data.Sqlite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OptionСlasses.SaveUpadateSheduleDB;

public class SUSheduleDb
{
    private static TimeSpan time=new TimeSpan(0,0,0,40);
    private long chatID;
    string shedule;
    public SUSheduleDb(long chatID, string shedule)
    {
        this.chatID = chatID;
        this.shedule = shedule;
    }
    public long ChatID { get { return chatID; } }
    public string Shedule { get { return shedule; } }
    private static void SetSheduleinDB(long chatID,string rasp)
    {
        using (var connection = new SqliteConnection(@"Data Source=AssistentData\AssistentBotDataBase.db"))
        { 
            connection.Open();
            string sql = $"REPLACE INTO ScheduleSaved (chatID,schedule) VALUES({chatID},'{rasp}')";
            SqliteCommand sqliteCommand = new SqliteCommand(sql, connection);
            sqliteCommand.ExecuteNonQuery();
        }
        
    }
    private static void ClearFliedShedule(long chatID, string rasp)
    {
        using (var connection = new SqliteConnection(@"Data Source=AssistentData\AssistentBotDataBase.db"))
        {
            connection.Open();
            string sql = $"REPLACE INTO ScheduleSaved (chatID,schedule) VALUES({chatID},NULL)";
            SqliteCommand sqliteCommand = new SqliteCommand(sql, connection);
            sqliteCommand.ExecuteNonQuery();
        }
        Console.WriteLine("Delete data");
    }
    public async void paradSetOfTimeClearShed()
    {
        SetSheduleinDB(ChatID, Shedule);
       // Console.WriteLine("!!!!!!!!!!!!!!!!!!!!!!!!!!!!");
        //var task = new Task(() => { Console.WriteLine("@@@@@"); });
        //task.ContinueWith(asd =>
        //{

        //    Task.Delay(time);
        //    ClearFliedShedule(ChatID, Shedule);
        //});
        //task.Start();
        var task = new Thread(() =>
          {
              Thread.Sleep(time);
              ClearFliedShedule(ChatID, Shedule);
          });
        task.Start();
        
    }

}

