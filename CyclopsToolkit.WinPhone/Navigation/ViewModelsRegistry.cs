using System;
using System.Collections.Generic;

namespace CyclopsToolkit.WinPhone.Navigation
{
    public static class ViewModelsRegistry
    {
        private static readonly Dictionary<Guid, object> ViewModels = new Dictionary<Guid, object>();

        public static string Register(object viewModel)
        {
            var id = Guid.NewGuid();
            ViewModels[id] = viewModel;
            return id.ToString();
        }

        public static object GetById(string id)
        {
            if (string.IsNullOrEmpty(id))
                return null;
            Guid vmId;
            if (!Guid.TryParse(id, out vmId))
            {
                //TODO: log
                return null;
            }
            object weakRef;
            if (ViewModels.TryGetValue(vmId, out weakRef) /*&& weakRef.IsAlive*/)
            {
                return weakRef;
            }
            //TODO: log
            return null;
        }
    }
}
