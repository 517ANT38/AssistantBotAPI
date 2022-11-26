using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OptionСlasses.Calendar;
 public class Day
{
    public Day(DayOfWeek name, int number)
    {
        Name = name; Number = number;
    }
    public DayOfWeek Name { get; set; }
    public int Number { get; set; }
}


