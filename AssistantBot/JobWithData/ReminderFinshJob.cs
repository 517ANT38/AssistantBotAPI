namespace AssistantBotAPI.JobWithData;

public class ReminderFinshJob
{
    public long ChatId { get; set; }   
    public int Hashe { get; set; }
    public bool Finish { get; set; }

    public ReminderFinshJob(long chatId, int hashe, bool finish)
    {
        ChatId = chatId;
        Hashe = hashe;
        Finish = finish;
    }
}

