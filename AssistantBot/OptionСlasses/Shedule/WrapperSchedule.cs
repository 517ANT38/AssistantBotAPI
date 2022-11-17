using JobWithData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OptionСlasses.Shedule;

public class WrapperSchedule : IAsyncLoaDatable
{
    IAsyncLoaDatable obj;
    private string RegexWUS => "[А-ЩЭ-ЯЁ][А-ЩЭ-ЯЁ][А-ЩЭ-ЯЁ]([А-ЩЭ-ЯЁ]{1,}|)";
    public WrapperSchedule(IAsyncLoaDatable obj)
    {
        this.obj = obj;
    }

    public async Task<WrapperAboveData<string>> LoadData()
    {
        throw new NotImplementedException();
    }
}

