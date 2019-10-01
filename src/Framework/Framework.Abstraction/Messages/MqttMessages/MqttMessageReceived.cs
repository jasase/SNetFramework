using Framework.Contracts.Extension.EventService;

namespace Framework.Contracts.Messages.MqttMessages
{
    public class MqttMessageReceived : EventMessage
    {
        public string Topic { get; set; }
        public byte[] Data { get; set; }
    }
}
