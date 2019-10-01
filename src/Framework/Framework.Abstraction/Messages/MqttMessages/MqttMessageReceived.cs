using Framework.Abstraction.Extension.EventService;

namespace Framework.Abstraction.Messages.MqttMessages
{
    public class MqttMessageReceived : EventMessage
    {
        public string Topic { get; set; }
        public byte[] Data { get; set; }
    }
}
