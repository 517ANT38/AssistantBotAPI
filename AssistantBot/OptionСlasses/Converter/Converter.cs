using AssistantBotAPI.OptionСlasses.fileProcessing;

namespace AssistantBotAPI.OptionСlasses.Converter;

public abstract class Converter : IReadable<string>, IWriteable<string>,IPerformable
{
    public abstract Task<string> ReadAsync(string nameFile);
    public abstract string ConvertName { get; }
    public abstract string ConvertToType { get; }

    public virtual async Task WriteAsync(string nameFile, string? value = null)
    {
        using (FileStream fs = File.Open(nameFile, FileMode.Create))
        {


            var b = System.Text.Encoding.UTF8.GetBytes(value);

            await fs.WriteAsync(b, 0, value.Length);
        }
    }
    public async Task PerformAsync(string nameFileIn,string nameFileOut)
    {
        var str=await ReadAsync(nameFileIn);
        await WriteAsync(nameFileOut,str);
    }
}

