using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AssistantBotAPI.OptionСlasses.fileProcessing
{
    public interface IWriteable<T>
    {
        Task WriteAsync(string nameFile,T? value=default);
    }
}
