using System;

namespace KuusouEngine
{
    public abstract partial class AEntity
    {
        public TComponent AddComponent<TComponent>() where TComponent : AComponent
        {
            return AddComponentCore(typeof(TComponent)) as TComponent;
        }
        public AComponent AddComponent(Type componentType)
        {
            if (!componentType.IsSubclassOf(typeof(AComponent)))
                return null;
            return AddComponentCore(componentType);
        }
        public void AddComponent(AComponent component)
        {
            Type componentType = component.GetType();
            if (Components.ContainsKey(componentType)) return;
            AddComponentCore(componentType, component);
        }
        public void AddComponent<TComponent>(TComponent component) where TComponent : AComponent
        {
            Type componentType = typeof(TComponent);
            if (Components.ContainsKey(componentType)) return;
            AddComponentCore(componentType, component);
        }
        private AComponent AddComponentCore(Type componentType)
        {
            if (Components.ContainsKey(componentType)) return null;
            AComponent component = Activator.CreateInstance(componentType, true) as AComponent;
            AddComponentCore(componentType, component);
            return component;
        }
        private void AddComponentCore(Type componentType, AComponent component)
        {
            component._parent = this;
            this.Components.Add(componentType, component);
            this.OnAddComponent(component);
        }
    }
}
