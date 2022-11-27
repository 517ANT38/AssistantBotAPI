using AssistantBotAPI.JobWithData;
using System.Text.Json;

namespace AssistantBotAPI.OptionСlasses.fileProcessing;

public class FileProcessing : IReadable<List<SaveCommandAndDat>>, IWriteable<List<SaveCommandAndDat>>
{
    //public List<SaveCommandAndDat> Process(List<SaveCommandAndDat> lst, SaveCommandAndDat value)
    //{
    //    List<SaveCommandAndDat> saves= new List<SaveCommandAndDat>(lst);
    //    saves.Add(value);
    //    return saves;
    //}

    public async Task<List<SaveCommandAndDat>?> ReadAsync(string nameFile)
    {
        using (FileStream fs = new FileStream(nameFile, FileMode.OpenOrCreate))
        {
            try
            {
                List<SaveCommandAndDat>? res = await JsonSerializer.DeserializeAsync<List<SaveCommandAndDat>>(fs);
                return res;
            }
            catch (Exception ex)
            {
                return null;
            }
        }
    }

    public async Task WriteAsync(string nameFile, List<SaveCommandAndDat> value)
    {
        using (FileStream fs = new FileStream(nameFile, FileMode.Truncate))
        {            
            await JsonSerializer.SerializeAsync<List<SaveCommandAndDat>>(fs, value);           
        }
    }
    public async Task perform(string nameFileOut, string nameFileIn,SaveCommandAndDat andDat) { 
        var s= await ReadAsync(nameFileIn);
        if (s != null) 
            s=new List<SaveCommandAndDat> { andDat };
        //var tmp= Process(s,andDat);
        await WriteAsync(nameFileOut, s); 
    }

}

