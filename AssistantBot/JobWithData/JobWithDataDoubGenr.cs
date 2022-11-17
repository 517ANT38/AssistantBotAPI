using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobWithData;
public class JobWithDataDoubGenr<T,V>
{
    private T _data;
    private V _v;
    public JobWithDataDoubGenr(T data, V v)
    {
        _data = data;
        _v = v;
    }
    public T Data { get { return _data; } }
    public V Vobt { get { return _v; } }
}
