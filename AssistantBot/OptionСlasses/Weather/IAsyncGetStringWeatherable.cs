using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OptionСlasses.Weather;

public interface IAsyncGetStringWeatherable
{
    Task<string> getStringWeather(string currenOrWeek = "Текущая");
}

