using System;
using System.Collections.Generic;

namespace Framework.EventCenter{
    /// <summary>
    /// The event center.
    /// </summary>
    /// <typeparam name="TKey">The key used to find a event.</typeparam>
    /// <typeparam name="TEvent">The event type to get.</typeparam>
    public class EventCenter<TKey, TEvent> where TEvent : Delegate
    {
        private Dictionary<TKey,TEvent> _events = new Dictionary<TKey, TEvent>();

        public void AddListener(TKey key, TEvent action){
            _events[key] = (TEvent)Delegate.Combine(_events[key], action);
        }

        public void RemoveListener(TKey key, TEvent action){
            Delegate.Remove(_events[key], action);
        }

        public void Trigger(TKey key, params object[] objects){
            _events[key].DynamicInvoke(objects);
        }
    }
}