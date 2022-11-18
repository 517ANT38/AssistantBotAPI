using ExceptionBot;
using JobWithData;
using Microsoft.Data.Sqlite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace OptionСlasses.Shedule;

public class WrapperSchedule : IAsyncLoaDatable
{
    
    private string RegexWUS => "[А-ЩЭ-ЯЁ][А-ЩЭ-ЯЁ][А-ЩЭ-ЯЁ]([А-ЩЭ-ЯЁ]{1,}|)";
    private List<string> paramsStr;
    public WrapperSchedule(string [] arr)
    {
        paramsStr =new List<string>(arr);
    }

    public async Task<WrapperAboveData<string>> LoadData()
    {
        IAsyncLoaDatable obj=null;
        string? url = null;
        bool fl = true;
        try
        {
            string[] tmp = paramsStr[0].Split(' ');

            if (Regex.IsMatch(tmp[1], RegexWUS, RegexOptions.IgnoreCase))
            {
                //Ищем вуз в базе данных
                url = getUrlUniversitySQLite(tmp[1]);
                if (url == null)
                    return new WrapperAboveData<string>("Такого вуза нет");
                if(tmp.Length <2)
                    return new WrapperAboveData<string>("Вуз не задан");
                if (tmp[1] == "СГТУ")
                {
                    if (Regex.IsMatch(paramsStr[1], ScheduleSSTU.RegStringChekData[0], RegexOptions.ECMAScript))
                    {
                        //if (paramsStr.Count > 1)
                        fl = !Regex.IsMatch(paramsStr[2], ScheduleSSTU.RegStringChekData[1], RegexOptions.IgnoreCase);

                        obj = new ScheduleSSTU(url, paramsStr[1], fl);
                    }
                    else
                        return new WrapperAboveData<string>("Неправильное формат название группы ");
                    
                }
                else if (tmp[1] == "СГУ")
                {
                    if (Regex.IsMatch(paramsStr[1], ScheduleCGU.RegStringChekData[0], RegexOptions.ECMAScript))
                    {
                        //if (paramsStr.Count > 1)
                        if (Regex.IsMatch(paramsStr[2], ScheduleSSTU.RegStringChekData[1], RegexOptions.ECMAScript))
                        {
                            if (Regex.IsMatch(paramsStr[3], ScheduleSSTU.RegStringChekData[2], RegexOptions.ECMAScript))
                            {
                                //if (paramsStr.Count > 1)
                                fl = !Regex.IsMatch(paramsStr[4], ScheduleSSTU.RegStringChekData[3], RegexOptions.IgnoreCase);

                                obj = new ScheduleCGU(url, paramsStr[1], paramsStr[2], paramsStr[3], fl);
                            }
                            else
                                return new WrapperAboveData<string>("Неправильное формат название группы ");

                        }
                        else
                            return new WrapperAboveData<string>("Неправильное формат название формы обучения ");

                    }
                    else
                        return new WrapperAboveData<string>("Неправильное формат факультета(института)");

                }

            }
            else
                return new WrapperAboveData<string>("Неправильный параметр названия вуза!");         
            

        }
        catch (IndexOutOfRangeException ex)
        {
            return new WrapperAboveData<string>("Проверьте параметры расписания,их передано слишком много!");
        }
        catch (GroupNotFoundException e)
        {
            return new WrapperAboveData<string>(e.Message);
        }
        catch (InvalidOperationException e)
        {
            return new WrapperAboveData<string>("Неправильный адрес сайта");
        }
        catch (HttpRequestException e)
        {
            return new WrapperAboveData<string>("Проверьте подключение к сети. Запустите команду еще раз,назвав группу и адрес сайта");
        }
        catch (TaskCanceledException e)
        {
            return new WrapperAboveData<string>("Время ожидания истекло. Запустите команду еще раз,назвав группу и адрес сайта");

        }
        return await obj.LoadData();
    }
    private static string getUrlUniversitySQLite(string p)
    {
        string? url = null;
        string command = $"SELECT url_u from University_url WHERE University_name='{p}'";
        using (var connection = new SqliteConnection(@"Data Source=C:\Users\user\source\repos\AssistantBot\AssistantBot\AssistentData\AssistentBotDataBase.db"))
        {
            connection.Open();
            SqliteCommand sqliteCommand = new SqliteCommand(command, connection);

            using (SqliteDataReader reader = sqliteCommand.ExecuteReader())
            {
                if (reader.HasRows) // если есть данные
                {

                    url = (string)reader.GetValue(0);

                }
            }
        }
        return url;
    }
}

