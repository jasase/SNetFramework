using Framework.Contracts.Extension;
using Framework.Contracts.Services.XmlToObject;
using System;
using System.Collections.Generic;
using System.IO;
using System.Security;
using System.Xml;
using System.Xml.Schema;

namespace XmlToObjectPlugin
{
    public class XmlToObject : IXmlToObject
    {
        private ILogger _logger;

        public XmlToObject(ILogger logger)
        {
            _logger = logger;
        }

        public T ReadXml<T>(string path)
        {
            return (T) ReadXml(path, typeof(T));
        }

        public object ReadXml(string path, Type type)
        {
            return ReadXml(new MyXmlReader(path), type);
        }

        public T ReadXml<T>(string path, IEnumerable<string> pathToSchema)
        {
            return (T)ReadXml(path, typeof(T), pathToSchema);
        }

        public T ReadXml<T>(Stream stream)
        {
            return (T)ReadXml(new MyXmlReader(stream), typeof (T));
        }

        public T ReadXmlFromString<T>(string xml)
        {
            var reader = new StringReader(xml);
            return (T) ReadXml(reader, typeof (T));
        }

        public object ReadXml(string path, Type type, IEnumerable<string> pathToSchema)
        {
            XmlReaderSettings settings = new XmlReaderSettings();            
            settings.ValidationType = ValidationType.Schema;
            settings.ValidationEventHandler += (sender, e) =>
            {
                var t = e;
            };
            foreach (var schema in pathToSchema)
            {
                if (File.Exists(schema))
                {
                    try
                    {
                        settings.Schemas.Add(XmlSchema.Read(XmlReader.Create(schema), null));
                    }
                    catch (SecurityException ex)
                    {
                        _logger.Error(ex, "Auf die angegebene XML-Schemadatei [{0}] konnte nicht zugegriefen werden",schema);
                    }
                    catch(XmlSchemaException ex)
                    {
                        _logger.Error(ex, "Die angegebene XML-Schemadatei [{0}] enthält Fehler", schema);
                    }
                }
                else
                {
                    _logger.Error("Die angegebene XML-Schemadatei [{0}] existiert nicht", schema);
                }
            }
            return ReadXml(XmlReader.Create(path, settings), type);
        }

        private object ReadXml(XmlReader stream, Type type)
        {
            var serializer = new MyXmlSerializer(type);                                    
            var result = serializer.Deserialize(stream);
            stream.Dispose();
            return result;
        }

        private object ReadXml(TextReader reader, Type type)
        {
            var serializer = new MyXmlSerializer(type);
            var result = serializer.Deserialize(reader);
            reader.Dispose();
            return result;
        }
    }
}
