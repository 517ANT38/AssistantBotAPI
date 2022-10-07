using System;

public static class StandardMessageBot 
{
	public const string messageHelp = "Чем могу я вам помочь ? ";
	public const string messageHello = "Привет, я ассистент бот";
	public const string messageHowYou = "Как ваши дела ? ";
	public const string messQuestSched = "Расписание за определенный день или за всю неделю?";
	public const string messQuestsGroup = "Какая у вас группа в вузе?";
	public const string messErrorGroup = "Такой группы нет!";
	public const string stickerNice = "https://raw.githubusercontent.com/517ANT38/imagesStikers/7f4da521aac29c0b73a95c63846d9a1bbbd77ba6/foto2.webp";
	public const string stickerNotNice = "https://raw.githubusercontent.com/517ANT38/imagesStikers/7f4da521aac29c0b73a95c63846d9a1bbbd77ba6/foto1.webp";
	public const string pattrenGroup = @"^(б|м|с)[1-2]{0,1}-([^\w\sъьЙйыЫЪЬ]{4})-[1-5]{2}";
	public const string pattrenGroup2 = @"^(б|м|с)[1-2]{0,1}-([^\w\sа-яЙйЫЪЬ]{4})-[1-5]{2}";

}
