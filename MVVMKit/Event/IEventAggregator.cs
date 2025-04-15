namespace MVVMKit.Event
{
    public interface IEventAggregator
    {
        TEventType GetEvent<TEventType>() where TEventType : EventBase, new();
    }
}
