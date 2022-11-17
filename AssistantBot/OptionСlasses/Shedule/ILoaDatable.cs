using JobWithData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OptionСlasses.Shedule;

public interface IAsyncLoaDatable
{
        Task<WrapperAboveData<string>> LoadData();
}

