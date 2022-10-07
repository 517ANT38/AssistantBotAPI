using HtmlAgilityPack;
using System;
using System.Net.Http.Json;

public class Schedule
{
	private string url;
	private string group;
    private string day;
	public Schedule(string url,string group,string day)
	{
		this.url = url;
		this.group = group;
        this.day = day;
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
    public string Day
    {
        get { return day; }
        set { day = value; }
    }
    private static async Task<string> GetMainSchedule(string url)
    {
        using var client = new HttpClient();
        var response = await client.GetAsync(url);
        Console.WriteLine(response);

        return await response.Content.ReadAsStringAsync();
        
    }
    private static async Task<string> creatUrlSchedule(string group, string url)
    {   
        var str=await GetMainSchedule(url);
        HtmlDocument doc = new HtmlDocument();
        doc.LoadHtml(str);
        var a = doc.DocumentNode.SelectNodes("//div[@class='col-auto group']");
        var atr="";
        foreach (var item in a)
        {
            if (item.ChildNodes[0].InnerText== group)
            {
                atr = item.ChildNodes[0].Attributes["href"].Value;
                break;
            }
                
        }
         
        using var client = new HttpClient();
        var response = await client.GetAsync(url+atr);
        return await response.Content.ReadAsStringAsync();
    }
  
    public  async void LoadData()
    {   
        var str = await creatUrlSchedule(group,url);
        HtmlDocument html = new HtmlDocument();
        html.LoadHtml(str);
        Console.WriteLine(html.DocumentNode.FirstChild);

        var container = html.DocumentNode.SelectSingleNode("//div[@class='container' and child::div[@class='calendar']]");
        Console.WriteLine(container==null);
        if (container != null)
        {
            var calendar = container.SelectSingleNode("//div[@class='container' and child::div[@class='calendar']]");
            var categories = calendar.SelectNodes("//div[@class='week']")
                                     .ToDictionary(k => k.InnerText, v => v.SelectSingleNode("//div[@class='day ']").SelectNodes("//div[@data-lesson]"));
            string textShedule = "";
            foreach (var category in categories)
            {
                //Console.WriteLine(category.Key);

                foreach (var weekday in category.Value)
                {
                    textShedule += weekday.SelectSingleNode("//div[@class='day-header']/div")?.InnerHtml.Replace("span", "").Trim('<','>').Replace("</>","\n  ");

                    Console.WriteLine($"- {textShedule}");
                    break;
                }
                break;
            }
        }
    }
}
