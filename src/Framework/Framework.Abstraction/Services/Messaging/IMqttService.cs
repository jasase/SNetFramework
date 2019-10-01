namespace Framework.Contracts.Services.Messaging
{
    public interface IMqttService
    {
        void RegisterTopic(string topic);

        void Publish(string topic, byte[] data);
    }
}
