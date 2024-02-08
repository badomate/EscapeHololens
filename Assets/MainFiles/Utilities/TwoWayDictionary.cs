

using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Utilities
{
    public class TwoWayDictionary<T1, T2>: IEnumerable<KeyValuePair<T1, T2>>
    {
        private readonly Dictionary<T1, T2> _forward;
        private readonly Dictionary<T2, T1> _reverse;
        public Dictionary<T1, T2>.KeyCollection Keys
        {
            get { return _forward.Keys; }
        }

        public int Count
        {
            get { return _forward.Count; }
        }

        public TwoWayDictionary()
        {
            _forward = new Dictionary<T1, T2>();
            _reverse = new Dictionary<T2, T1>();
        }
        public TwoWayDictionary(Dictionary<T1, T2> dictionary)
        {
            _forward = new Dictionary<T1, T2>(dictionary);
            _reverse = dictionary.ToDictionary((i) => i.Value, (i) => i.Key);
        }

        public void Add(T1 key, T2 value)
        {
            _forward.Add(key, value);
            _reverse.Add(value, key);
        }

        public void Update(T1 key, T2 value)
        {
            if (_forward.ContainsKey(key))
            {
                T2 oldValue = _forward[key];
                _forward[key] = value;
                _reverse.Remove(oldValue);
                _reverse.Add(value, key);
            }
            else
            {
                Add(key, value);
            }
        }

        public bool Remove(T1 key)
        {
            T2 value = _forward[key];
            return _forward.Remove(key) && _reverse.Remove(value);
        }

        public void Remove(T2 key)
        {
            T1 value = _reverse[key];
            _reverse.Remove(key);
            _forward.Remove(value);
        }

        public bool ContainsKey(T1 key)
        {
            return _forward.ContainsKey(key);
        }
        public bool ContainsKey(T2 key)
        {
            return _reverse.ContainsKey(key);
        }

        public bool TryGetValue(T1 key, out T2 value)
        {
            return _forward.TryGetValue(key, out value);
        }

        public bool TryGetValue(T2 key, out T1 value)
        {
            return _reverse.TryGetValue(key, out value);
        }

        public T2 GetValue(T1 key)
        {
            return _forward.GetValueOrDefault(key);
        }

        public T1 GetValue(T2 key)
        {
            return _reverse.GetValueOrDefault(key);
        }

        public void Clear()
        {
            _forward.Clear();
            _reverse.Clear();
        }

        public IEnumerator<KeyValuePair<T1, T2>> GetEnumerator()
        {
            return ((IEnumerable<KeyValuePair<T1, T2>>)_forward).GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return ((IEnumerable)_forward).GetEnumerator();
        }
    }
}