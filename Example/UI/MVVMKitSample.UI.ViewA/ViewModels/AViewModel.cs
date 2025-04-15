using System;
using MVVMKit.Commands;
using MVVMKit.Event;
using MVVMKit.MVVM;
using MVVMKit.Regions;
using MVVMKitSample.Application.ServiceA;
using MVVMKitSample.UI.Core.Events;

namespace MVVMKitSample.UI.ViewA.ViewModels
{
    public class AViewModel : NotifyBase, INavigationAware
    {
        private string _sendMessage;
        private readonly IEventAggregator _ea;

        public string SendMessage { get => _sendMessage; set => SetProperty(ref _sendMessage, value); }
        public IServiceA ServiceA { get; }

        public BindingCommand BtnSendClickCommand { get; }

        public AViewModel(IServiceA serviceA, IEventAggregator ea)
        {
            ServiceA = serviceA;
            _ea = ea;

            BtnSendClickCommand = new BindingCommand(OnBtnSendClick);
        }

        private void OnBtnSendClick()
        {
            _ea.GetEvent<TextEvent>()?.Publish(SendMessage);
        }

        public void OnNavigatedTo()
        {
        }
        public void OnNavigatedFrom()
        {
        }
    }
}
