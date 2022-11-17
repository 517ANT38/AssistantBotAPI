using JobWithData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace OptionСlasses.Shedule;

public class WrapperSchedule : IAsyncLoaDatable
{
    private IAsyncLoaDatable obj;
    private string RegexWUS => "[А-ЩЭ-ЯЁ][А-ЩЭ-ЯЁ][А-ЩЭ-ЯЁ]([А-ЩЭ-ЯЁ]{1,}|)";
    private List<string> paramsStr;
    public WrapperSchedule(string [] arr)
    {
        paramsStr =new List<string>(arr);
    }

    public async Task<WrapperAboveData<string>> LoadData()
    {

        string? url = null;
        string? group = null;
        bool fl = true;
        try
        {
            string[] tmp = paramsStr[0].Split(' ');

            if (Regex.IsMatch(tmp[1], RegexWUS, RegexOptions.IgnoreCase))
            {
                //Ищем вуз в базе данных
                url = "";
                if (url == null)
                    return new WrapperAboveData<string>("Такого вуза нет");
                if(tmp.Length <2)
                    return new WrapperAboveData<string>("Вуз не задан");
                if (tmp[1] == "СГТУ")
                {
                    if (Regex.IsMatch(paramsStr[1], ScheduleSSTU.RegStringChekData[0], RegexOptions.ECMAScript))
                    {
                        group = paramsStr[1];
                    }
                    else
                        return new WrapperAboveData<string>("Неправильное формат название группы ");
                    if(paramsStr.Count>1)
                        fl = !Regex.IsMatch(paramsStr[2],ScheduleSSTU.RegStringChekData[1], RegexOptions.IgnoreCase);

                    obj = new ScheduleSSTU(url, group, fl);
                }
                else if (tmp[1] == "СГУ")
                {

                }

            }
            else
                return new WrapperAboveData<string>("Неправильный параметр вуза!");         
            

        }
        catch (IndexOutOfRangeException ex)
        {
            return new WrapperAboveData<string>("Проверьте параметры расписания!");
        }
        return await obj.LoadData();
    }
}

