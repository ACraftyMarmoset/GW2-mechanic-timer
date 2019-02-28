using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace MechanicTimer.Utilities
{
    class ObservableDictionary<K, V> : IDictionary<K, V>, INotifyPropertyChanged, INotifyCollectionChanged
    {
        private Dictionary<K, V> _dictionary = new Dictionary<K, V>();

        public ICollection<K> Keys => ((IDictionary<K, V>)_dictionary).Keys;

        public ICollection<V> Values => ((IDictionary<K, V>)_dictionary).Values;

        public int Count => ((IDictionary<K, V>)_dictionary).Count;

        public bool IsReadOnly => ((IDictionary<K, V>)_dictionary).IsReadOnly;

        public V this[K key]
        {
            get
            {
                return ((IDictionary<K, V>)_dictionary)[key];
            }
            set
            {
                var newItem = new KeyValuePair<K, V>(key, value);
                var oldItem = new KeyValuePair<K, V>(key, _dictionary[key]);

                ((IDictionary<K, V>)_dictionary)[key] = value;

                var args = new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Replace, newItem, oldItem);
                NotifyCollectionChanged(args);
            }
        }

        public bool ContainsKey(K key)
        {
            return ((IDictionary<K, V>)_dictionary).ContainsKey(key);
        }

        public void Add(K key, V value)
        {
            ((IDictionary<K, V>)_dictionary).Add(key, value);
            var args = new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Add, new KeyValuePair<K, V>(key, value));
            NotifyCollectionChanged(args);
        }

        public bool Remove(K key)
        {
            bool result = false;
            if (_dictionary.ContainsKey(key))
            {
                var item = _dictionary[key];
                result = ((IDictionary<K, V>)_dictionary).Remove(key);
                if (result)
                {
                    var args = new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Remove, new KeyValuePair<K, V>(key, item));
                    NotifyCollectionChanged(args);
                }
            }
            return result;
        }

        public bool TryGetValue(K key, out V value)
        {
            return ((IDictionary<K, V>)_dictionary).TryGetValue(key, out value);
        }

        public void Add(KeyValuePair<K, V> item)
        {
            ((IDictionary<K, V>)_dictionary).Add(item);
            var args = new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Add, item);
            NotifyCollectionChanged(args);
        }

        public void Clear()
        {
            ((IDictionary<K, V>)_dictionary).Clear();
            var args = new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset);
            NotifyCollectionChanged(args);
        }

        public bool Contains(KeyValuePair<K, V> item)
        {
            return ((IDictionary<K, V>)_dictionary).Contains(item);
        }

        public void CopyTo(KeyValuePair<K, V>[] array, int arrayIndex)
        {
            ((IDictionary<K, V>)_dictionary).CopyTo(array, arrayIndex);
        }

        public bool Remove(KeyValuePair<K, V> item)
        {
            bool result = ((IDictionary<K, V>)_dictionary).Remove(item);
            if (result)
            {
                var args = new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Remove, item);
                NotifyCollectionChanged(args);
            }
            return result;
        }

        public IEnumerator<KeyValuePair<K, V>> GetEnumerator()
        {
            return ((IDictionary<K, V>)_dictionary).GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return ((IDictionary<K, V>)_dictionary).GetEnumerator();
        }

        public event PropertyChangedEventHandler PropertyChanged;
        private void NotifyPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public event NotifyCollectionChangedEventHandler CollectionChanged;
        private void NotifyCollectionChanged(NotifyCollectionChangedEventArgs args)
        {
            CollectionChanged?.Invoke(this, args);
        }
    }
}
