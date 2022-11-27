namespace AssistantBotAPI.OptionСlasses.Shedule;

public interface IAsyncLoaDatable
{
        Task<(bool,string)> LoadData();
}

