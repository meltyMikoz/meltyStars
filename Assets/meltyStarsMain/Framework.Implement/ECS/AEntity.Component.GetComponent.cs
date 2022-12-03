using System;

namespace MeltyStars
{
    public abstract partial class AEntity
    {
        public TComponent GetComponent<TComponent>() where TComponent : AComponent
        {
            return GetComponentCore(typeof(TComponent)) as TComponent;
        }
        public AComponent GetComponent(Type componentType)
        {
            if (!componentType.IsSubclassOf(typeof(AComponent)))
                return null;
            return GetComponentCore(componentType);
        }
        private AComponent GetComponentCore(Type componentType)
        {
            if (!Components.ContainsKey(componentType)) return null;
            return Components[componentType];
        }
    }
}
