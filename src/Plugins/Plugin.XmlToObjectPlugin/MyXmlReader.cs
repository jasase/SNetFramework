using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace XmlToObjectPlugin
{
    class MyXmlReader : XmlTextReader
    {
        private bool _readingDate = false;
        const string CUSTOM_UTC_DATE_TIME_FORMAT = "ddd, dd MMM yyyy HH:mm:ss K"; // Thu, 13 Dec 2012 21:02:00 GMT

        

        public MyXmlReader(Stream s) : base(s) { }

        public MyXmlReader(string inputUri) : base(inputUri) { }

        public override DateTime ReadContentAsDateTime()
            => base.ReadContentAsDateTime();

        public override object ReadContentAs(Type returnType, IXmlNamespaceResolver namespaceResolver)
            => base.ReadContentAs(returnType, namespaceResolver);

        public override object ReadElementContentAs(Type returnType, IXmlNamespaceResolver namespaceResolver)
            => base.ReadElementContentAs(returnType, namespaceResolver);

        public override DateTime ReadElementContentAsDateTime()
            => base.ReadElementContentAsDateTime();

        public override DateTime ReadElementContentAsDateTime(string localName, string namespaceURI)
            => base.ReadElementContentAsDateTime(localName, namespaceURI);

        public override void ReadStartElement()
        {            
            var localName = LocalName;
            var nUri = NamespaceURI;
            if (string.Equals(base.NamespaceURI, string.Empty, StringComparison.InvariantCultureIgnoreCase) &&
                (string.Equals(base.LocalName, "lastBuildDate", StringComparison.InvariantCultureIgnoreCase) ||
                string.Equals(base.LocalName, "pubDate", StringComparison.InvariantCultureIgnoreCase)))
            {
                _readingDate = true;
            }
            base.ReadStartElement();            
        }

        public override void ReadEndElement()
        {
            if (_readingDate)
            {
                _readingDate = false;
            }
            base.ReadEndElement();
        }

        public override string ReadString()
        {
            if (_readingDate)
            {
                var dateString = base.ReadString();
                var dt = DateTime.Now;
                if (!string.IsNullOrEmpty(dateString))
                {
                    if (!DateTime.TryParse(dateString, out dt))
                    {
                        dt = DateTime.ParseExact(dateString, CUSTOM_UTC_DATE_TIME_FORMAT, CultureInfo.InvariantCulture);
                    }
                }
                return dt.ToUniversalTime().ToString("R", CultureInfo.InvariantCulture);
            }
            else
            {
                return base.ReadString();
            }
        }
    }
}
