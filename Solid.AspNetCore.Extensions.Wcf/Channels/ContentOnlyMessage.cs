using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel.Channels;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace Solid.AspNetCore.Extensions.Wcf.Channels
{
    internal abstract class ContentOnlyMessage : Message
    {
        MessageHeaders headers;
        MessageProperties properties;

        protected ContentOnlyMessage()
        {
            this.headers = new MessageHeaders(MessageVersion.None);
        }

        public override MessageHeaders Headers
        {
            get
            {
                if (IsDisposed)
                    throw new ObjectDisposedException("ContentOnlyMessage");

                return this.headers;
            }
        }

        public override MessageProperties Properties
        {
            get
            {
                if (IsDisposed)
                    throw new ObjectDisposedException("ContentOnlyMessage");

                if (this.properties == null)
                    this.properties = new MessageProperties();

                return this.properties;
            }
        }

        public override MessageVersion Version
        {
            get
            {
                return headers.MessageVersion;
            }
        }

        protected override void OnBodyToString(XmlDictionaryWriter writer)
        {
            OnWriteBodyContents(writer);
        }
    }
}
