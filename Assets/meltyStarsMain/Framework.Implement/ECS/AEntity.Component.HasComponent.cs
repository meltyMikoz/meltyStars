using System;

namespace KuusouEngine
{
    public abstract partial class AEntity
    {
        public bool HasComponent<TComponent>()
        {
            return Components.ContainsKey(typeof(TComponent));
        }
        public bool HasComponent(Type componentType)
        {
            return Components.ContainsKey(componentType);
        }
    }
}
