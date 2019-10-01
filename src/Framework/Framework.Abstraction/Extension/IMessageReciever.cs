using Framework.Abstraction.Extension.EventService;

namespace Framework.Abstraction.Extension
{
  public interface IMessageReceiver<in TMessage>
    where TMessage : EventMessage
  {
    void Update(TMessage message);
  }
}
