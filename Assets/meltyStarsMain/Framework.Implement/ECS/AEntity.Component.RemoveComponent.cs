using System;

namespace KuusouEngine
{
    public abstract partial class AEntity
    {
        public void RemoveComponent(AComponent component)
        {
            RemoveComponent(component.GetType());
        }
        public void RemoveComponent<TComponent>() where TComponent : AComponent
        {
            RemoveComponentCore(typeof(TComponent));
        }
        public void RemoveComponent(Type componentType)
        {
            if (!componentType.IsSubclassOf(typeof(AComponent)))
                return;
            RemoveComponentCore(componentType);
        }
        private void RemoveComponentCore(Type componentType)
        {
            if (!Components.ContainsKey(componentType)) return;
            AComponent component = Components[componentType];
            component._parent = null;
            this.Components.Remove(componentType);
            //触发OnRemoveComponent
            this.OnRemoveComponent(component);
        }
    }
}
