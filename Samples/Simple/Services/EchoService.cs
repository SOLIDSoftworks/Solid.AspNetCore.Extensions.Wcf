using Simple.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Simple.Services
{
    public class EchoService : IEchoService
    {
        public T Echo<T>(T value)
        {
            return value;
        }
    }
}
