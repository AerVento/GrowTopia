using System;

namespace Framework.EventCenter{
    public interface IReadOnlyEventCenter<TKey, TEvent> where TEvent:Delegate{
        public void AddListener(TKey key, TEvent action);
        public void RemoveListener(TKey key, TEvent action);
    }
}