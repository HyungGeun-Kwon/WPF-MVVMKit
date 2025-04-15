using System;
using System.Collections.Generic;

namespace MVVMKit.Event
{
    public class PubSubEvent<T> : EventBase
    {
        private readonly List<Action<T>> _handlers = new List<Action<T>>();
        public void Publish(T @parameter)
        {
            foreach (var handler in _handlers)
            {
                handler?.Invoke(parameter);
            }
        }
        public void Subscribe(Action<T> action)
        {
            _handlers.Add(action);
        }
        public void Unsubscribe(Action<T> action)
        {
            if (action == null) return;
            _handlers.Remove(action);
        }
    }
    public class PubSubEvent : EventBase
    {
        private readonly List<Action> _handlers = new List<Action>();
        public void Publish()
        {
            foreach (var handler in _handlers)
            {
                handler?.Invoke();
            }
        }
        public void Subscribe(Action action)
        {
            _handlers.Add(action);
        }
        public void Unsubscribe(Action action)
        {
            if (action == null) return;
            _handlers.Remove(action);
        }
    }
}
