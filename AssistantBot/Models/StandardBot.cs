using AssistantBotAPI.Models.Commands;
using AssistantBotAPI.OptionСlasses.Converter;
using System;
using System.Collections.ObjectModel;

public static  class StandardBot 
{
	public const string messageHelp = "Чем могу я вам помочь ? ";
	public const string messageHello = "Привет, я ассистент бот! Как ваши дела?";
	public const string destinationFilePath = @"FileUsersMessageSended\FileInU\File.file";
    public const string outFile = @"FileUsersMessageSended\FileOutU\File";
    public const string messQuestSched = "Расписание за определенный день или за всю неделю?";
	public const string messQuestsGroup = "Какая у вас группа в вузе?";
	public const string messErrorGroup = "Такой группы нет!";
	public const string stickerNice = "https://raw.githubusercontent.com/517ANT38/imagesStikers/7f4da521aac29c0b73a95c63846d9a1bbbd77ba6/foto2.webp";
	public const string stickerNotNice = "https://raw.githubusercontent.com/517ANT38/imagesStikers/7f4da521aac29c0b73a95c63846d9a1bbbd77ba6/foto1.webp";
	public const string pattrenGroup = @"^(б|м|с)[1-2]{0,1}-([^\w\sъьЙйыЫЪЬ]{4})-[1-5]{2}";
	public const string pattrenGroup2 = @"^(б|м|с)[1-2]{0,1}-([^\w\sа-яЙйЫЪЬ]{4})-[1-5]{2}";
    public const string patternUri = @"^(?:(http(s|)|ftp)?:\/\/)[\w.-]+(?:\.[\w\.-]+)+[\w\-\._~:\/?#[\]@!\$&'\(\)\*\+,;=.]+$";
    public const string nameResFile = "ResultFile";
    private static ReadOnlyCollection<Command> commandsList = new List<Command>()
    {
        new HelpCommand(),
        new StartCommand(),
        new ScheduleCommand(),
        new WeatherCommand(),
        new ReminderCommand(),
        new ConverterCommand(),
        new FunCommand()

    }.AsReadOnly();
    private static ReadOnlyCollection<Converter> converters=new List<Converter>()
    {
        new ConverterDocxInPDF(),
        new ConverterJsonInXml(),
        new ConverterXmlInJsoncs(),

    }.AsReadOnly();
    public static ReadOnlyCollection<Command> CommandsList { get => commandsList;  }
    public static ReadOnlyCollection<Converter> Converters { get => converters;  }
}
    
