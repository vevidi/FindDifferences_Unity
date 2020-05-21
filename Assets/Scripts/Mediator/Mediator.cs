using System;
using System.Collections.Generic;
using Vevidi.FindDiff.GameMediator.Commands;

namespace Vevidi.FindDiff.GameMediator
{
    public class Mediator
    {
        public delegate void MediatorCallback<T>(T c) where T : Command;

        private Dictionary<Type, Delegate> _subscribers = new Dictionary<Type, Delegate>();

        public void Subscribe<T>(MediatorCallback<T> callback) where T : Command
        {
            if (callback == null)
                throw new ArgumentNullException("Mediator Subscribe error: subscriber is null");

            var tp = typeof(T);
            if (_subscribers.ContainsKey(tp))
                _subscribers[tp] = Delegate.Combine(_subscribers[tp], callback);
            else
                _subscribers.Add(tp, callback);
        }

        public void DeleteSubscriber<T>(MediatorCallback<T> callback) where T : Command
        {
            if (callback == null)
                throw new ArgumentNullException("Mediator DeleteSubscriber error: subscriber is null");

            var tp = typeof(T);
            if (_subscribers.ContainsKey(tp))
            {
                var d = _subscribers[tp];
                d = Delegate.Remove(d, callback);
                if (d == null) _subscribers.Remove(tp);
                else _subscribers[tp] = d;
            }
        }

        public void Publish<T>(T c) where T : Command
        {
            var tp = typeof(T);
            if (_subscribers.ContainsKey(tp))
                _subscribers[tp].DynamicInvoke(c);
        }
    }
}