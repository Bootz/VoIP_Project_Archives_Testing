using System.Collections.Generic;
using System.Linq;
using Autofac;
using VoipTranslator.Client.Core.Common;
using VoipTranslator.Protocol;

namespace VoipTranslator.Client.Core.Compasition
{
    public class LayerModule : Module
    {
        protected sealed override void Load(ContainerBuilder builder)
        {
            ChildModules.ForEach(i => i.Load(builder));
            OnMap(builder);
            base.Load(builder);
        }

        protected virtual void OnMap(ContainerBuilder builder)
        {
        }

        protected virtual IEnumerable<LayerModule> ChildModules
        {
            get { return Enumerable.Empty<LayerModule>(); }
        }

        public virtual void OnPostContainerBuild(IContainer container)
        {
        }
    }
}
