using AssistantBotAPI.JobWithData;
using Newtonsoft.Json;

namespace AssistantBotAPI.OptionСlasses.fileProcessing;

public class FileProcesSaveComAndDat : IReadable<List<SaveCommandAndDat>>, IWriteable<List<SaveCommandAndDat>>
{
    
    public async Task<List<SaveCommandAndDat>?> ReadAsync(string nameFile)
    {
        using (FileStream fs = File.Open(nameFile,FileMode.OpenOrCreate))
        {
            if(fs.Length<0)
                return null;
            byte[] data = new byte[fs.Length];
            
            await fs.ReadAsync(data, 0, data.Length);
            var s = System.Text.Encoding.UTF8.GetString(data, 0, data.Length);
            
            List<SaveCommandAndDat> res = JsonConvert.DeserializeObject<List<SaveCommandAndDat>>(s);
            
            return res;
        }
    }

    public async Task WriteAsync(string nameFile, List<SaveCommandAndDat> value)
    {
            using (FileStream fs = File.Open(nameFile,FileMode.Create))
            {
            
            var s= JsonConvert.SerializeObject(value);
                var b= System.Text.Encoding.UTF8.GetBytes(s);
                
                await fs.WriteAsync(b, 0, s.Length);
            }
    }
    public async Task perform(string nameFileOut, string nameFileIn,SaveCommandAndDat andDat) { 
        var s= await ReadAsync(nameFileIn);
        await WriteAsync(nameFileOut, s); 
    }

}

