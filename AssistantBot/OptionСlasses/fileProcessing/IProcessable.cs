using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AssistantBotAPI.OptionСlasses.fileProcessing;

public interface IProcessable<T,V>
{
    T Process(T value1,V? value2=default);
}

