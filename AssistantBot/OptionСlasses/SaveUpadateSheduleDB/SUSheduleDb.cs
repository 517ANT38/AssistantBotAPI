using Microsoft.Data.Sqlite;

namespace AssistantBotAPI.OptionСlasses.SaveUpadateSheduleDB;

public class SUSheduleDb
{
    private static int days_p=0;
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
    public static void ClearFliedShedule()
    {
        using (var connection = new SqliteConnection(@"Data Source=AssistentData\AssistentBotDataBase.db"))
        {
            connection.Open();
            string sql = $"DELETE FROM ScheduleSaved  WHERE (date_p<=date('now','-{days_p} day'));";
            SqliteCommand sqliteCommand = new SqliteCommand(sql, connection);
            sqliteCommand.ExecuteNonQuery();

        }
        
    }
    
    

}

