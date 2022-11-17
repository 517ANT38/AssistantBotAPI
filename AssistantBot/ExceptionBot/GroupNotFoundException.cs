using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExceptionBot;

public  class GroupNotFoundException : Exception
{
    public GroupNotFoundException(string m) : base(m) { }
}


