using AssistantBotAPI.JobWithData;
using Newtonsoft.Json;

namespace AssistantBotAPI.OptionСlasses.fileProcessing;

public class FileFinishThreadCl : IReadable<List<ReminderFinshJob>>, IWriteable<List<ReminderFinshJob>>
{
    public async Task<List<ReminderFinshJob>?> ReadAsync(string nameFile)
    {
        using (FileStream fs = File.Open(nameFile, FileMode.OpenOrCreate))
        {
            if (fs.Length < 0)
                return null;
            byte[] data = new byte[fs.Length];

            await fs.ReadAsync(data, 0, data.Length);
            var s = System.Text.Encoding.UTF8.GetString(data, 0, data.Length);

            List<ReminderFinshJob> res = JsonConvert.DeserializeObject<List<ReminderFinshJob>>(s);

            return res;
        }
    }

    public async Task WriteAsync(string nameFile, List<ReminderFinshJob>? value = null)
    {
        using (FileStream fs = File.Open(nameFile, FileMode.Create))
        {

            var s = JsonConvert.SerializeObject(value);
            var b = System.Text.Encoding.UTF8.GetBytes(s);

            await fs.WriteAsync(b, 0, s.Length);
        }
    }
}

