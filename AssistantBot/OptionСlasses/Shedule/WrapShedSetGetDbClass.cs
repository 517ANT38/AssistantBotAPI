using JobWithData;
using Microsoft.Data.Sqlite;
using OptionСlasses.SaveUpadateSheduleDB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OptionСlasses.Shedule;

public class WrapShedSetGetDbClass : IAsyncLoaDatable
{
    private long chatId;
    private string[] arr;
    
    public WrapShedSetGetDbClass(long chatId,string[] arr)
    {
        this.chatId = chatId;
        this.arr = arr;
        
    }
    public async Task<(bool,string)> LoadData()
    {
        var tmpRes = GetSheduleinDB(chatId,arr);
        if (tmpRes.Item1)
        {
            return (true,tmpRes.Item2);
        }
        else
        {
            WrapperSchedule schedule = new WrapperSchedule(arr);
            var data = await schedule.LoadData();
            if (data.Item1)
            {
                int hash=HashCodeForBD(String.Join(" ",arr) + chatId.ToString());
                SUSheduleDb sU = new SUSheduleDb(hash, data.Item2);
                sU.paradSetOfTimeClearShed();
            }
            
            return data;
        }
        return (false,null);
    }
    private static (bool, string) GetSheduleinDB(long chatID,string[] arr)
    {

        using (var connection = new SqliteConnection(@"Data Source=AssistentData\AssistentBotDataBase.db"))
        {
            connection.Open();
            int hash = HashCodeForBD(String.Join(" ", arr)+chatID.ToString());
            string sql = $"SELECT schedule FROM ScheduleSaved WHERE hash_p={hash}";
            SqliteCommand sqliteCommand = new SqliteCommand(sql, connection);
            using (SqliteDataReader reader = sqliteCommand.ExecuteReader())
            {
                if (reader.Read()) // если есть данные
                {
                    
                    var a = reader.GetValue(0);
                    if (a.GetType() == typeof(DBNull))
                    {
                        return (false,"Данных нет");
                    }

                    return (true, (string)a);


                }
                else
                {
                    return (false, null);
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
}

