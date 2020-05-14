using System;
using System.Collections.Generic;

public delegate void MediatorCallback<T>(T c) where T : ICommand;

public class Mediator
{
    private Dictionary<Type, Delegate> _subscribers = new Dictionary<Type, Delegate>();

    public void Subscribe<T>(MediatorCallback<T> callback) where T : ICommand
    {
        if (callback == null) throw new ArgumentNullException("callback");
        var tp = typeof(T);
        if (_subscribers.ContainsKey(tp))
            _subscribers[tp] = Delegate.Combine(_subscribers[tp], callback);
        else
            _subscribers.Add(tp, callback);
    }

    public void DeleteSubscriber<T>(MediatorCallback<T> callback) where T : ICommand
    {
        if (callback == null) throw new ArgumentNullException("callback");
        var tp = typeof(T);
        if (_subscribers.ContainsKey(tp))
        {
            var d = _subscribers[tp];
            d = Delegate.Remove(d, callback);
            if (d == null) _subscribers.Remove(tp);
            else _subscribers[tp] = d;
        }
    }

    public void Publish<T>(T c) where T : ICommand
    {
        var tp = typeof(T);
        if (_subscribers.ContainsKey(tp))
        {
            _subscribers[tp].DynamicInvoke(c);
        }
    }
}