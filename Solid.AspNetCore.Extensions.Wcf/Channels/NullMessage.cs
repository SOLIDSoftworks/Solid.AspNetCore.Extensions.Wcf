using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Solid.AspNetCore.Extensions.Wcf.Channels
{
    internal class NullMessage : StringMessage
    {
        public NullMessage()
            : base(string.Empty)
        {
        }
    }
}
