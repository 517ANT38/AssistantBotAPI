using JobWithData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OptionСlasses.Shedule;

public class ScheduleCGU:IAsyncLoaDatable
{
    string fakult;
    string group;
    ScheduleCGU(string fakult,string group)
    {
        this.fakult = fakult;
        this.group = group;
    }

    public async Task<WrapperAboveData<string>> LoadData()
    {
        throw new NotImplementedException();
    }
}

