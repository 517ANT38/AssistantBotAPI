using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AssistantBotAPI.OptionСlasses.Converter;

public interface IPerformable
{
    Task PerformAsync(string nameFileIn, string nameFileOut);
}

