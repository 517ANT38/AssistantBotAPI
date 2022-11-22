using JobWithData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OptionСlasses.Shedule;

public class ScheduleCGU:IAsyncLoaDatable
{
    private string fakult;
    private string group;
    private string form;
    private string url;
    private bool fl;
    public static List<string> RegStringChekData
    {
        get
        {
            List<string> ts = new List<string>()
            {
          //      @"^(?:(http(s|)|ftp)?:\/\/)[\w.-]+(?:\.[\w\.-]+)+[\w\-\._~:\/?#[\]@!\$&'\(\)\*\+,;=.]+$",
                @"^(Институт|институт|факультет|Факультет|колледж|Колледж|)[А-Я ]{1,}(факультет|)",
                @"(Дневная|Заочная|Вечерняя|дневная|заочная|вечерняя)( форма|)( обучения|)",
                @"[0-9]{1,7}",
                @"[Нн]а сегодня"
            };
            return ts;
        }
    }
    public ScheduleCGU(string url,string fakult,string group,string form,bool fl=true)
    {
        this.fakult = fakult;
        this.group = group;
        this.form = form;
        this.url = url;
        this.fl = fl;
    }

    public async Task<(bool,string)> LoadData()
    {
        throw new NotImplementedException();
    }
}

