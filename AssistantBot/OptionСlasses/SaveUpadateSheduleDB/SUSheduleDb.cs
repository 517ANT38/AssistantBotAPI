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
    private int hash;
    private string shedule;
    public SUSheduleDb(int hash, string shedule)
    {
        this.hash = hash;
        this.shedule = shedule;
    }
    public int Hash { get { return hash; } }
    public string Shedule { get { return shedule; } }
    public  void SetSheduleinDB()
    {
        using (var connection = new SqliteConnection(@"Data Source=AssistentData\AssistentBotDataBase.db"))
        { 
            connection.Open();
            string sql = $"REPLACE INTO ScheduleSaved (date_p,hash_p,schedule) VALUES(date('now'),{hash},'{shedule}')";
            SqliteCommand sqliteCommand = new SqliteCommand(sql, connection);
            sqliteCommand.ExecuteNonQuery();
        }
        
    }
    
    

}

