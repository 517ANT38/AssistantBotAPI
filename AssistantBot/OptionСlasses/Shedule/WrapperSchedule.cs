using ExceptionBot;
using Microsoft.Data.Sqlite;
using System.Text.RegularExpressions;

namespace AssistantBotAPI.OptionСlasses.Shedule;

public class WrapperSchedule : IAsyncLoaDatable
{
    
    private string RegexWUS => "[А-ЩЭ-ЯЁ][А-ЩЭ-ЯЁ][А-ЩЭ-ЯЁ]([А-ЩЭ-ЯЁ]{1,}|)";
    private List<string> paramsStr;
    public WrapperSchedule(string [] arr)
    {
        paramsStr =new List<string>(arr);
    }

    public async Task<(bool,string)> LoadData()
    {
        IAsyncLoaDatable obj=null;
        string? url = null;
        bool fl = true;
        try
        {
            //string[] tmp = paramsStr[0].Split(' ');
            
            if (Regex.IsMatch(paramsStr[0], RegexWUS, RegexOptions.IgnoreCase))
            {
                //Ищем вуз в базе данных
                url = getUrlUniversitySQLite(paramsStr[0]);
                if (url == null)
                    return (false,"Такого вуза нет");
                //if(tmp.Length <2)
                //    return ("Вуз не задан");
                if (paramsStr[0] == "СГТУ")
                {
                    
                    if (Regex.IsMatch(paramsStr[1], ScheduleSSTU.RegStringChekData[0], RegexOptions.ECMAScript))
                    {
                        if (paramsStr.Count > 2)
                        {
                            //Console.WriteLine(paramsStr[1]);
                            //string a=String.Join(" ", paramsStr[2], paramsStr[3]);
                            fl = !Regex.IsMatch(paramsStr[2], ScheduleSSTU.RegStringChekData[1], RegexOptions.IgnoreCase);
                            Console.WriteLine(fl);
                        }

                        obj = new ScheduleSSTU(url, paramsStr[1], fl);
                    }
                    else
                    {
                        Console.WriteLine(paramsStr[1]);
                        return (false,"Неправильное формат название группы ");
                    }
                }
                else if (paramsStr[0] == "СГУ")
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
                                return (false,"Неправильное формат название группы ");

                        }
                        else
                            return (false,"Неправильное формат название формы обучения ");

                    }
                    else
                        return (false,"Неправильное формат факультета(института)");

                }

            }
            else
                return (false,"Неправильный параметр названия вуза!");         
            

        }
        catch(ArgumentOutOfRangeException ex)
        {
            return (false, "Непередано ни одного аргумента для подбора расписания!");
        }
        catch (IndexOutOfRangeException ex)
        {
            return (false,"Проверьте параметры расписания,их передано слишком много!");
        }
        catch (GroupNotFoundException e)
        {
            return (false,e.Message);
        }
        catch (InvalidOperationException e)
        {
            return (false,"Неправильный адрес сайта");
        }
        catch (HttpRequestException e)
        {
            return (false,"Проверьте подключение к сети. Запустите команду еще раз,назвав группу и адрес сайта");
        }
        catch (TaskCanceledException e)
        {
            return (false,"Время ожидания истекло. Запустите команду еще раз,назвав группу и адрес сайта");

        }
        return await obj.LoadData();
    }
    private static string getUrlUniversitySQLite(string p)
    {
        string? url = null;
        string command = $"SELECT url_u from University_url WHERE University_name='{p}'";
        using (var connection = new SqliteConnection(@"Data Source=AssistentData\AssistentBotDataBase.db"))
        {
            connection.Open();
            SqliteCommand sqliteCommand = new SqliteCommand(command, connection);

            using (SqliteDataReader reader = sqliteCommand.ExecuteReader())
            {
                if (reader.Read()) // если есть данные
                {
                    
                    url = (string)reader.GetValue(0);

                }
            }
        }
        return url;
    }
   
}

