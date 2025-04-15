using System;
using System.Windows.Media;
using MVVMKit.Event;
using MVVMKit.MVVM;
using MVVMKit.Regions;
using MVVMKitSample.Application.ServiceA;
using MVVMKitSample.UI.Core.Events;

namespace MVVMKitSample.UI.ViewB.ViewModels
{
    public class BViewModel : NotifyBase, INavigationAware
    {
        private readonly IEventAggregator _ea;
        private string _receivedmessage;
        public string ReceivedMessage { get => _receivedmessage; set => SetProperty(ref _receivedmessage, value); }

        public IServiceA ServiceA { get; }

        public BViewModel(IServiceA serviceA, IEventAggregator ea)
        {
            ServiceA = serviceA;
            _ea = ea;
        }

        private void MessageReceived(string message)
        {
            ReceivedMessage = message;
        }

        public void OnNavigatedTo()
        {
            _ea.GetEvent<TextEvent>().Subscribe(MessageReceived);
        }
        public void OnNavigatedFrom()
        {
            _ea.GetEvent<TextEvent>().Unsubscribe(MessageReceived);
        }
    }
}
