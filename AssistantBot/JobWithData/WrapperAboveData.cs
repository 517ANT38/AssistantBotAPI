using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobWithData;

public class WrapperAboveData 
{
    private List<string> obt;

    private string v;
    public WrapperAboveData(params string[] ts):this(ts.ToList()){}
    public WrapperAboveData(List<string> ts)
    {
        if (ts.Count < 2)
        {
            v = ts[0];
            obt = null;
        }
        else
        {
            obt = new List<string>(ts);
            v = null;
        }
    }
    public string ToString()
    {
        return null;
    }
    
    

}

