using System.Collections.Generic;

namespace WeatherMonitor.Domain
{
    public interface IKeyValueStore<TKey, TValue>
    {
        IEnumerable<TKey> GetAllKeys();
        TValue GetValue(TKey key);
        void UpdateValue(TKey key, TValue value);
    }
}