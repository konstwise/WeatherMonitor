using System.Collections.Concurrent;
using System.Collections.Generic;
using WeatherMonitor.Domain;

namespace WeatherMonitor.KeyValueStore
{
    /// <summary>
    /// Primitive yet thread-safe in-memory key-value store implementation
    /// </summary>
    /// <typeparam name="TKey"></typeparam>
    /// <typeparam name="TValue"></typeparam>
    public class InMemoryKeyValueStore<TKey, TValue> : IKeyValueStore<TKey, TValue>
    {
        private readonly ConcurrentDictionary<TKey, TValue> _internalStore =
            new();

        public IEnumerable<TKey> GetAllKeys()
        {
            return _internalStore.Keys;
        }

        public TValue GetValue(TKey key)
        {
            return _internalStore[key];
        }

        public void UpdateValue(TKey key, TValue value)
        {
            _internalStore[key] = value;
        }
    }
}