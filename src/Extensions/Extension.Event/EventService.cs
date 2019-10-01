using Framework.Abstraction.Extension;
using Framework.Abstraction.Extension.EventService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Extension.Event
{
    public class EventService : IEventService
    {
        private Dictionary<Type, object> _manager;
        private object _managerDictionaryLock;
        private ILogger _logger;

        public EventService(ILogManager logManager)
        {
            _manager = new Dictionary<Type, object>();
            _managerDictionaryLock = new object();
            _logger = logManager.GetLogger(GetType());
        }

        private SpecificEventMessageManager<TMessage> GetOrCreate<TMessage>() where TMessage : EventMessage
        {
            SpecificEventMessageManager<TMessage> manager;
            Type messageType = typeof(TMessage);
            lock (_managerDictionaryLock)
            {
                if (_manager.ContainsKey(messageType) && _manager[messageType] is SpecificEventMessageManager<TMessage>)
                {
                    manager = (SpecificEventMessageManager<TMessage>)_manager[messageType];
                }
                else
                {
                    manager = new SpecificEventMessageManager<TMessage>();
                    _manager.Add(messageType, manager);
                }
                return manager;
            }
        }

        public void Register<TMessage>(Action<TMessage> message) where TMessage : EventMessage
        {
            _logger.Debug("Register message reciever {0}", message);
            GetOrCreate<TMessage>().Register(message);
        }

        public void Register<TMessage>(IMessageReceiver<TMessage> messageReviever) where TMessage : EventMessage
        {
            _logger.Debug("Register message reciever {0}", messageReviever);
            GetOrCreate<TMessage>().Register(messageReviever);
        }

        public void Deregister<TMessage>(Action<TMessage> message) where TMessage : EventMessage
        {
            _logger.Debug("Deregister message reciever {0}", message);
            GetOrCreate<TMessage>().Derigster(message);
        }

        public void Deregister<TMessage>(IMessageReceiver<TMessage> messageReviever) where TMessage : EventMessage
        {
            _logger.Debug("Deregister message reciever {0}", messageReviever);
            GetOrCreate<TMessage>().Derigster(messageReviever);
        }

        public void Publish<TMessage>(TMessage message) where TMessage : EventMessage
        {
            _logger.Debug("Publishing message of type [{0}] - {1}", message.GetType().FullName, message.ToString());
            GetOrCreate<TMessage>().Publish(message);
        }
    }
}
