using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace Solid.AspNetCore.Extensions.Wcf.Channels
{
    internal class StringMessage : ContentOnlyMessage
    {
        string data;

        public StringMessage(string data)
            : base()
        {
            this.data = data;
        }

        public override bool IsEmpty
        {
            get
            {
                return String.IsNullOrEmpty(this.data);
            }
        }

        protected override void OnWriteBodyContents(XmlDictionaryWriter writer)
        {
            if (data != null && data.Length > 0)
            {
                writer.WriteElementString("BODY", data);
            }
        }
    }
}
