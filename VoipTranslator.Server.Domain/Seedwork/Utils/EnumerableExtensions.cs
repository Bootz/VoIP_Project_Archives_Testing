using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace VoipTranslator.Server.Domain.Seedwork.Utils //common namespace to see these methods everywhere without usings
{
    public static class EnumerableExtensions
    {        
        public static void ForEach<T>(this IEnumerable<T> source, Action<T> action)
        {
            foreach (var item in source) 
            {
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

        public static bool Any(this IEnumerable source) 
        {
            foreach (var item in source) 
            {
                return true; //just one call of MoveNext is needed
            }
            return false;
        }

        public static IEnumerable<T> OfNotType<T>(this IEnumerable<T> source, Type typeToIgnore)
        {
            return source.Where(item => item.GetType() != typeToIgnore);
        }

        public static bool IsNullOrEmpty(this IEnumerable enumerable) 
        {
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

        public static string CutIfTooLong(this string source, int maxLength = 1000)
        {
            if (string.IsNullOrEmpty(source))
                return source;
            if (source.Length < maxLength)
                return source;
            return source.Remove(maxLength - 1);
        }

        public static DateTime ToTime(this DateTime dateTime)
        {
            return new DateTime(1900, 1, 1, dateTime.Hour, dateTime.Minute, dateTime.Second);
        }
    }
}

