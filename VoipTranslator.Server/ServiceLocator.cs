using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Autofac;

namespace VoipTranslator.Server
{
    public static class ServiceLocator
    {
        private static readonly Dictionary<object[], WeakReference> Cache = new Dictionary<object[], WeakReference>();

        public static void Init(IContainer container)
        {
            Instance = container;
        }

        public static IContainer Instance { get; private set; }
        
        [DebuggerStepThrough]
        public static T Resolve<T>()
        {
            return Instance.Resolve<T>();
        }

        [DebuggerStepThrough]
        public static T ResolveWith<T>(params object[] parameters) where T : class
        {
            return Instance.ResolveOptional<T>(parameters.Select(i => new TypedParameter(i.GetType(), i)));
        }

        [DebuggerStepThrough]
        public static T ResolveWithCache<T>(params object[] parameters) where T : class
        {
            lock (Cache)
            {
                foreach (var cacheItem in Cache.ToList())
                {
                    if (cacheItem.Key.SequenceEqual(parameters) && cacheItem.Value.IsAlive)
                    {
                        return (T)cacheItem.Value.Target;
                    }
                }
                var instance = ResolveWith<T>(parameters);
                Cache[parameters] = new WeakReference(instance);
                return instance;
            }
        }
    }
}
