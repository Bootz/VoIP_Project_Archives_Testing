using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace VoipTranslator.Client.Core.Common
{
    public static class EnumerableExtensions
    {
        public static void ForEach<T>(this IEnumerable<T> source, Action<T> action) {
            foreach (var item in source) {
                action(item);
            }
        }

        public static T GetAndRemove<T>(this IDictionary<object,object> source, object key)
        {
            object value;
            if (source.TryGetValue(key, out value))
            {
                source.Remove(key);
                return (T) value;
            }
            else
            {
                throw new KeyNotFoundException(string.Format("object with key {0} doesn't exist", key));       
            }
        }

        public static bool Any(this IEnumerable source) {
            foreach (var item in source) {
                return true; //just one call of MoveNext is needed
            }
            return false;
        }

        public static IEnumerable<T> OfNotType<T>(this IEnumerable<T> source, Type typeToIgnore)
        {
            return source.Where(item => item.GetType() != typeToIgnore);
        }

        public static bool IsNullOrEmpty(this IEnumerable enumerable) {
            return enumerable == null || !Any(enumerable);
        }

        public static T PreLastOrDefault<T>(this IEnumerable<T> source)
        {
            if (source == null)
                throw new ArgumentNullException();
            var list = source.ToList();
            if (list.Count == 0)
            {
                return default(T);
            }
            if (list.Count == 1)
            {
                return list[0];
            }
            return list[list.Count - 2];
        }

        public static void RemoveAll<T>(this Collection<T> collection, Func<T, bool> selector)
        {
            for (int i = 0; i < collection.Count; i++)
            {
                var item = collection[i];
                if (selector(item))
                {
                    collection.Remove(item);
                    i--;
                }
            }
        }

        public static void InsertFirstIfUnique(this IList list, object obj)
        {
            if (list.Count == 0)
            {
                list.Insert(0, obj);
            }
            else if (!list[0].Equals(obj))
            {
                list.Insert(0, obj);
            }
        }

        public static void InvokeIfNotNull(this Action action)
        {
            if (action != null)
            {
                action.Invoke();
            }
        }

        public static string ToParamsString<K, V>(this IEnumerable<KeyValuePair<K, V>> source, string separator)
        {
            return source.Aggregate("", (acc, kvp) => kvp.Key + "=" + kvp.Value + separator);
        }
    }
}
