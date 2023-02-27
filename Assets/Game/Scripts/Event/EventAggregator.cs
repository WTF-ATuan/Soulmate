using UniRx;
using System;
using System.Threading.Tasks;

public class EventAggregator
{
    static IMessageBroker messageBroker;
    static IMessageBroker MessageBroker => messageBroker ??  (messageBroker = new MessageBroker());

    public static void Publish<T>(T message)
    { MessageBroker.Publish(message); }

    public static IObservable<T> OnEvent<T>()
    { return MessageBroker.Receive<T>(); }
}