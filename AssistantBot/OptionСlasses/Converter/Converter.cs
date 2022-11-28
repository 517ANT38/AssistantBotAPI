using AssistantBotAPI.OptionСlasses.fileProcessing;

namespace AssistantBotAPI.OptionСlasses.Converter;

public abstract class Converter : IReadable<string>, IWriteable<string>
{
    public abstract Task<string> ReadAsync(string nameFile);
    public abstract string ConvertName { get; }
    public abstract string ConvertToType { get; }

    public abstract  Task WriteAsync(string nameFile, string? value);
    
    public async Task PerformAsync(string nameFileIn,string nameFileOut)
    {
        var str=await ReadAsync(nameFileIn);
        await WriteAsync(nameFileOut,str);
    }
}

