using System;
using Framework.Abstraction.Extension.EventService;

namespace Framework.Abstraction.Extension
{
  public interface IEventService
  {
    void Register<TMessage>(Action<TMessage> message)
      where TMessage : EventMessage;
    void Register<TMessage>(IMessageReceiver<TMessage> messageReviever)
      where TMessage : EventMessage;

    void Deregister<TMessage>(Action<TMessage> message)
      where TMessage : EventMessage;
    void Deregister<TMessage>(IMessageReceiver<TMessage> messageReviever)
      where TMessage : EventMessage;

    void Publish<TMessage>(TMessage message)
      where TMessage : EventMessage;
  }
}
