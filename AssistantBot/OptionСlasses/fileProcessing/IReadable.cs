using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AssistantBotAPI.OptionСlasses.fileProcessing;

public interface IReadable<T>
{
    Task<T>  ReadAsync(string nameFile);
}

