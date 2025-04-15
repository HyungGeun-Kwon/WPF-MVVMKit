using System;
using System.Collections.Concurrent;

namespace MVVMKit.Event
{
    public class EventAggregator : IEventAggregator
    {
        private readonly ConcurrentDictionary<Type, EventBase> _events = new ConcurrentDictionary<Type, EventBase>();

        public TEventType GetEvent<TEventType>() where TEventType : EventBase, new()
        {
            if (!_events.TryGetValue(typeof(TEventType), out var existingEvent))
            {
                var newEvent = new TEventType();
                _events.TryAdd(typeof(TEventType), newEvent);
                return newEvent;
            }
            return (TEventType)existingEvent;
        }
    }
}
