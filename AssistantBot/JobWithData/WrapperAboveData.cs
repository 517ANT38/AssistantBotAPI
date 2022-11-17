using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobWithData;

public class WrapperAboveData<T> 
{
    private List<T> obt;

    private T v;
    public WrapperAboveData(params T[] ts):this(ts.ToList()){}
    public WrapperAboveData(List<T> ts)
    {
        if (ts.Count < 2)
        {
            v = ts[0];
            obt = null;
        }
        else
        {
            obt = new List<T>(ts);
            v = default(T);
        }
    }
    public List<T> GetData()
    {
        if(obt == null)
        {
            obt = new List<T>() {v};
        }
        return new List<T>(obt);
    }
    
    

}

