using Framework.Contracts.Extension;
using Framework.Contracts.Extension.EventService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventExtension
{
    //TODO Multithreading Safe machen
    public class SpecificEventMessageManager<TMessage>
      where TMessage : EventMessage
    {
        private List<IMessageReceiver<TMessage>> _reciever;
        private List<Action<TMessage>> _actions;

        public SpecificEventMessageManager()
        {
            _reciever = new List<IMessageReceiver<TMessage>>();
            _actions = new List<Action<TMessage>>();
        }

        public void Register(Action<TMessage> message)
        {
            if (!_actions.Contains(message))
            {
                _actions.Add(message);
            }
        }

        public void Register(IMessageReceiver<TMessage> messageReviever)
        {
            if (!_reciever.Contains(messageReviever))
            {
                _reciever.Add(messageReviever);
            }
        }

        public void Derigster(Action<TMessage> message)
        {
            if (_actions.Contains(message))
            {
                _actions.Remove(message);
            }
        }

        public void Derigster(IMessageReceiver<TMessage> messageReviever)
        {
            if (_reciever.Contains(messageReviever))
            {
                _reciever.Remove(messageReviever);
            }
        }

        public void Publish(TMessage message)
        {
            foreach (var action in _actions)
            {
                action(message);
            }
            foreach (var reciever in _reciever)
            {
                reciever.Update(message);
            }            
        }
    }
}
