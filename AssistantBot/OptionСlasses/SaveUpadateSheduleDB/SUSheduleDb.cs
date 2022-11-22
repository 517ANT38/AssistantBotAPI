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
    private int hash;
    private string shedule;
    public SUSheduleDb(int hash, string shedule)
    {
        this.hash = hash;
        this.shedule = shedule;
    }
    public int Hash { get { return hash; } }
    public string Shedule { get { return shedule; } }
    private static void SetSheduleinDB(int hash,string rasp)
    {
        using (var connection = new SqliteConnection(@"Data Source=AssistentData\AssistentBotDataBase.db"))
        { 
            connection.Open();
            string sql = $"REPLACE INTO ScheduleSaved (hash_p,schedule) VALUES({hash},'{rasp}')";
            SqliteCommand sqliteCommand = new SqliteCommand(sql, connection);
            sqliteCommand.ExecuteNonQuery();
        }
        
    }
    private static void ClearFliedShedule(int hash)
    {
        using (var connection = new SqliteConnection(@"Data Source=AssistentData\AssistentBotDataBase.db"))
        {
            connection.Open();
            string sql = $"REPLACE INTO ScheduleSaved (hash_p,schedule) VALUES({hash},NULL)";
            SqliteCommand sqliteCommand = new SqliteCommand(sql, connection);
            sqliteCommand.ExecuteNonQuery();
        }
        Console.WriteLine("Delete data");
    }
    public  void paradSetOfTimeClearShed()
    {
        SetSheduleinDB(Hash, Shedule);
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
              ClearFliedShedule(Hash);
          });
        task.Start();
        
    }
    

}

