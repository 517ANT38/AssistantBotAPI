using HtmlAgilityPack;
using System;
using System.Net.Http.Json;
using Fizzler.Systems.HtmlAgilityPack;
using JobWithData;
using ExceptionBot;
using System.Text.RegularExpressions;
using System.Text;

namespace OptionСlasses.Shedule;
public class ScheduleSSTU: IAsyncLoaDatable
{
    
    private string url;
    private string group;
    private bool flag;
    public static List<string> RegStringChekData
    {
        get
        {
            List<string> ts = new List<string>()
            {
          //      @"^(?:(http(s|)|ftp)?:\/\/)[\w.-]+(?:\.[\w\.-]+)+[\w\-\._~:\/?#[\]@!\$&'\(\)\*\+,;=.]+$",
                @"^(б|м|с)[1-5]{0,1}-([^\w\sа-яЙйЫЪЬ]{3,4}(оз|з|ипу|озипу|))-[1-5]{2}(| )",
               @"[Нн]а сегодня"
            };
            return ts;
        }
    }
    public ScheduleSSTU(string url, string group, bool flag=true)
    {
        this.url = url;
        this.group = group;
        this.flag = flag;
    }
    public string Url
    {
        get { return url; }

    }

    public string Group
    {
        get { return group; }
        set { group = value; }
    }
    public bool Flag { get { return flag; } }
    private static async Task<string> GetMainSchedule(string url)
    {
        using var client = new HttpClient();
        var response = await client.GetAsync(url);
        Console.WriteLine(response);

        return await response.Content.ReadAsStringAsync();

    }
    private static async Task<string> creatUrlSchedule(string group, string url)
    {
        
        string str = await GetMainSchedule(url);
        
        var doc = new HtmlDocument();
        doc.LoadHtml(str);
        var document = doc.DocumentNode;
        var a = document.QuerySelectorAll(".col-auto.group");
        string? atr =null;
        foreach (var item in a)
        {
            if (item.ChildNodes[0].InnerText == group)
            {
                atr = item.ChildNodes[0].Attributes["href"].Value;
                break;
            }

        }
        if (atr == null)
            throw new GroupNotFoundException("Группа не найдена");
        using var client = new HttpClient();
        var response = await client.GetAsync(url + atr);
        return await response.Content.ReadAsStringAsync();
    }

    public async Task<WrapperAboveData<string>> LoadData()
    {
        string str = await creatUrlSchedule(group, url);
        
        
        var html = new HtmlDocument();
        html.LoadHtml(str);
        var document = html.DocumentNode;
        var nodes = getListNodeSelectClass(document.QuerySelector(".week").ChildNodes);
        if (nodes == null) { return new WrapperAboveData<string>("Расписание не найдено"); }

        Console.WriteLine(nodes[0].InnerHtml);
        Dictionary<DayOfWeek, string> res = new Dictionary<DayOfWeek, string>()
        {
            [DayOfWeek.Monday] =null,
            [DayOfWeek.Tuesday] =null,
            [DayOfWeek.Wednesday] =null,
            [DayOfWeek.Thursday] =null,
            [DayOfWeek.Friday] =null,
            [DayOfWeek.Saturday] =null,

        };
        
        for (int i = 0; i < nodes.Count; i++)
        {
            string tmp=nodes[i].QuerySelector(".day-header>div>span").InnerText;
            Console.WriteLine(tmp);
            res[getDay(tmp)]= HtmlToPlainText(nodes[i]);
            
        }
        
        if(flag)
            return new WrapperAboveData<string>(new List<string>(res.Values));
        if(DateTime.Now.DayOfWeek!= DayOfWeek.Sunday)
            return new WrapperAboveData<string>(res[DateTime.Now.DayOfWeek]);
        else return new WrapperAboveData<string>("Расписания на воскресенье нет");
    }
    private  static DayOfWeek getDay(string str)
    {
        switch (str)
        {
            case "Понедельник":
                return DayOfWeek.Monday;
            case "Вторник":
                return DayOfWeek.Tuesday;
            case "Среда":
                return DayOfWeek.Wednesday;
            case "Четверг":
                return DayOfWeek.Thursday;
            case "Пятница":
                return DayOfWeek.Friday;
            case "Суббота":
                return DayOfWeek.Saturday;
            default:
                throw new Exception("Такого дня недели нет в рассписании");
        }
    }
    private static List<HtmlNode> getListNodeSelectClass(HtmlNodeCollection htmlNodes)
    {
        List<HtmlNode> nodes = new List<HtmlNode>();
        if (htmlNodes == null) return null;
        foreach (HtmlNode node in htmlNodes)
        {
            var tmp = new List<string>(node.GetClasses());
            if (tmp.Count() == 1 && tmp[0] == "day")
                nodes.Add(node);
            else if (tmp.Count() == 2 && tmp[0] == "day" && tmp[1] == "day-current")
                nodes.Add(node);
        }
        return nodes;
    }
    
    private static string HtmlToPlainText(HtmlNode html)
    {

        StringBuilder text = new StringBuilder();
        foreach (var item in html.SelectNodes(".//text()"))
        {
            text.Append(" "+ TextBearbeiten(item.InnerText));
            
        }
        text=text.Replace("&mdash;", "").Replace("&nbsp;","");
        
        return text.ToString();
    }
    private static string TextBearbeiten(string str)
    {
        
        Dictionary<Regex,string> arr = new Dictionary<Regex, string>()
        {
            [new Regex(@"Иностранный язык")] = $"{Environment.NewLine}" + "<i>           " + str + $"</i>{Environment.NewLine}",
            [new Regex(@"([1-2][0-9]|0[0-9]|3[1-2])\.(0[0-9]|1[0-2])")]= "<i>                              " + str+ $"          </i>{Environment.NewLine}{Environment.NewLine}",
            [new Regex(@"^((([0-1]|)[0-9]|2[1-3]):[0-5][0-9] - (([0-1]|)[0-9]|2[1-3]):[0-5][0-9])")]= "<i>                   " + str + "</i>",
            [new Regex(@"((([0-9]|[1-2][0-5])\/[0-7][0-9][0-9])|СЗ-[1-2]|ВК[0-9])")] = "<i>  (" + str + $") </i>{Environment.NewLine}",
            [new Regex(@"(Понедельник|Вторник|Среда|Четверг|Пятница|Суббота)")] = $"-------------------------------------------------------------------------{Environment.NewLine}<b>                            " + str + $"</b>{Environment.NewLine}",
            [new Regex(@"(\(прак\)|\(лекц\))")] = "<i>                        " + str + $"</i>{Environment.NewLine}",
            [new Regex(@"[А-ЩЭ-ЯЁ][а-яё]{1,} [А-ЩЭ-ЯЁ][а-яё]{1,9}( [А-ЩЭ-ЯЁ][а-яё]{1,9}|)")]= "<u>         " + str + $"</u>{Environment.NewLine}{Environment.NewLine}",
            [new Regex(@"[А-ЩЭ-ЯЁ][а-яёА-ЩЭ-ЯЁ\-]{1,} [А-ЩЭ-ЯЁа-яё\-]{1,9}( [а-яёА-ЩЭ-ЯЁ\-]{1,9}|)")]= "<i>           " + str + $"</i>{Environment.NewLine}",
            [new Regex(@"Подгр. [0-9]")]= "                      " + str ,
            
        };
        foreach (var item in arr)
        {
            if(item.Key.IsMatch(str))
                return item.Value;
        }
        return str;
    }
}
