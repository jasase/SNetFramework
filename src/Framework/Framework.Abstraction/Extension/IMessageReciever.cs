using Framework.Contracts.Extension.EventService;

namespace Framework.Contracts.Extension
{
  public interface IMessageReceiver<in TMessage>
    where TMessage : EventMessage
  {
    void Update(TMessage message);
  }
}
