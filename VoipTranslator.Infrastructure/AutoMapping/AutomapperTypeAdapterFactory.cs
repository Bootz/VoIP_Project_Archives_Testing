using System;
using System.Linq;
using System.Collections.Generic;
using System.Reflection;
using AutoMapper;

namespace VoipTranslator.Infrastructure.AutoMapping
{
    public class AutomapperTypeAdapterFactory
    {
        #region Constructor

        /// <summary>
        /// Create a new Automapper type adapter factory
        /// </summary>
        public static void LoadTypes(IEnumerable<Assembly> assemblies)
        {
            //scan all assemblies finding Automapper Profile
            var profiles = assemblies.SelectMany(a => a.DefinedTypes).Where(t => t.BaseType == typeof(Profile));

            Mapper.Initialize(cfg =>
            {
                foreach (var item in profiles)
                {
                    if (item.FullName != "AutoMapper.SelfProfiler`2")
                        cfg.AddProfile(Activator.CreateInstance(item.AsType()) as Profile);
                }
            });
        }

        #endregion

        #region ITypeAdapterFactory Members

        public static AutomapperTypeAdapter Create()
        {
            return new AutomapperTypeAdapter();
        }

        #endregion
    }
}
