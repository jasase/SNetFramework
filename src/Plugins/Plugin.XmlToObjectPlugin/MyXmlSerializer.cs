using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace XmlToObjectPlugin
{
    public class MyXmlSerializer : XmlSerializer
    {
        public MyXmlSerializer(Type type)
            : base(type)
        { }

        public override bool CanDeserialize(System.Xml.XmlReader xmlReader)
        {
            return base.CanDeserialize(xmlReader);
        }

        protected override XmlSerializationReader CreateReader()
        {
            var temp = base.CreateReader();
            return temp;
        }

        protected override object Deserialize(XmlSerializationReader reader)
        {
            return base.Deserialize(reader);
        }
    }
}
