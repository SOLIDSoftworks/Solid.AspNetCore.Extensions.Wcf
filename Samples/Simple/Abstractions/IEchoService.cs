using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Simple.Abstractions
{
    public interface IEchoService
    {
        T Echo<T>(T value);
    }
}
