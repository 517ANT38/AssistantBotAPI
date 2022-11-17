using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace JobWithData;
public class Property
{
    private Object obt;
    private string name;
    public Property(Object obt,string name)
    {
        this.obt = obt; 
        this.name = name;
    }
    public Object Obt { get { return obt; } }
    public string Name { get { return name; } }
}

