using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OptionСlasses.Calendar;
public class Month
{
    public Month(MonthName monthName, int year)
    {
        Name = monthName;
        Year = year;
        var leapyear = Year % 4 == 0;
        var days = Name == MonthName.February ? (leapyear ? 29 : 28) : (Name == MonthName.April || Name == MonthName.June || Name == MonthName.September || Name == MonthName.November ? 30 : 31);
        Days = new Day[days];
        var firstday = year * 365 + (leapyear ? -1 : 0) + (((year - (year % 4)) / 4)) - (((year - (year % 400)) / 400)) + 3;
        var month = (int)monthName;
        firstday += month < 1 ? 0 : 31;
        firstday += month < 2 ? 0 : (leapyear ? 29 : 28);
        firstday += month < 3 ? 0 : 31;
        firstday += month < 4 ? 0 : 30;
        firstday += month < 5 ? 0 : 31;
        firstday += month < 6 ? 0 : 30;
        firstday += month < 7 ? 0 : 31;
        firstday += month < 8 ? 0 : 31;
        firstday += month < 9 ? 0 : 30;
        firstday += month < 10 ? 0 : 31;
        firstday += month < 11 ? 0 : 30;
        firstday = firstday % 7;
        for (int i = 0; i < Days.Length; i++)
            Days[i] = new Day((DayOfWeek)((i + firstday) % 7), (int)(i + 1));
    }
    public int Year { get; set; }
    public MonthName Name { get; set; }
    public Day[] Days { get; set; }
    public int Weeks
    {
        get
        {
            var days = (int)Days[0].Name + Days.Length - 1;
            return (int)(((days - (days % 7)) / 7) + (days % 7 > 0 ? 1 : 0));
        }
    }
    public static int ConverterInMonthName(string str)
    {
        switch (str)
        {
            case "January":
                return 1;
            case "February":
                return 2;
            case "March":
                return 3;
            case "April":
                return 4;
            case "May":
                return 5;
            case "June":
                return 6;
            case "July":
                return 7;
            case "August":
                return 8;
            case "September":
                return 9;
            case "October":
                return 10;
            case "November":
                return 11;
            case "December":
                return 12;
            default:
                return 0;
        }
    }
}

