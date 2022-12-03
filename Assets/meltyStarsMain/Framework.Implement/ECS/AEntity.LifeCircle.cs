using System;
using System.Linq;

namespace MeltyStars
{
    public abstract partial class AEntity
    {
        public void Awake() { OnAwake(); }
        public void Update() { OnUpdate(); }
        public void FixedUpdate() { OnFixedUpdate(); }
        public void LateUpdate() { OnLateUpdate(); }
        public void Destroy(bool destroyChildren = true, bool pushIntoPool = true) { OnDestroy(destroyChildren, pushIntoPool); }
        private void OnAwake()
        {
            //初始化容器
            this.Components = new System.Collections.Generic.Dictionary<Type, AComponent>();
            this.Children = new System.Collections.Generic.Dictionary<Type, System.Collections.Generic.Dictionary<string, AEntity>>();
            if (!this.Enabled)
                (this as IAwake)?.OnAwakeCore();
            this.Enabled = true;
            foreach (var component in Components)
            {
                component.Value.Awake();
            }
            foreach (var kv1 in Children)
            {
                foreach (var kv2 in kv1.Value)
                {
                    kv2.Value.Awake();
                }
            }
        }
        private void OnUpdate()
        {
            if (!this.Enabled) return;
            (this as IUpdate)?.OnUpdateCore();
            foreach (var component in Components)
            {
                component.Value.Update();
            }
            foreach (var kv1 in Children)
            {
                foreach (var kv2 in kv1.Value)
                {
                    kv2.Value.Update();
                }
            }
        }
        private void OnFixedUpdate()
        {
            if (!this.Enabled) return;
            (this as IFixedUpdate)?.OnFixedUpdateCore();
            foreach (var component in Components)
            {
                component.Value.FixedUpdate();
            }
            foreach (var kv1 in Children)
            {
                foreach (var kv2 in kv1.Value)
                {
                    kv2.Value.FixedUpdate();
                }
            }
        }
        private void OnLateUpdate()
        {
            if (!this.Enabled) return;
            (this as ILateUpdate)?.OnLateUpdateCore();
            foreach (var component in Components)
            {
                component.Value.LateUpdate();
            }
            foreach (var kv1 in Children)
            {
                foreach (var kv2 in kv1.Value)
                {
                    kv2.Value.LateUpdate();
                }
            }
        }
        private void OnDestroy(bool destroyChildren, bool pushIntoPool)
        {
            if (this.Enabled)
                (this as IDestroy)?.OnDestroyCore();
            this.Enabled = false;
            while (Components.Count > 0)
            {
                Components.ElementAt(0).Value.Destroy();
            }
            if (destroyChildren)
            {
                while (Children.Count > 0)
                {
                    while (Children.ElementAt(0).Value.Count > 0)
                    {
                        Children.ElementAt(0).Value
                                .ElementAt(0).Value
                                .Destroy(destroyChildren);
                    }
                }
            }
            //一定是要放在Destroy之后 因为有可能会压进引用池
            if (Parent)
            {
                if (IsComponent())
                    Parent.RemoveComponent(this.GetType());
                else
                    Parent.RemoveChild(this);
            }
            //置空容器
            this.Children = null;
            this.Components = null;
            //压入对象池
            if (IsComponent())
            {
                //抛出异常
                return;
            }
            if (pushIntoPool)
                ObjectScheduler.Instance.Push<AEntity>(this);
        }
        private void OnAddComponent(AComponent component)
        {
            (this as IAddComponent)?.OnAddComponentCore(component);
            component.OnAddedAsComponent(this);
            EventScheduler.Instance.Dispatch(new EventType.OnAddComponent() { Entity = this, Component = component });
        }
        private void OnRemoveComponent(AComponent component)
        {
            (this as IRemoveComponent)?.OnRemoveComponentCore(component);
            component.OnRemovedFromComponent(this);
            component.Destroy();
            EventScheduler.Instance.Dispatch(new EventType.OnRemoveComponent() { Entity = this, Component = component });
        }
        private void OnAddChild(AEntity childEntity)
        {
            (this as IAddChild)?.OnAddChildCore(childEntity);
            (childEntity as IAddedAsChild)?.OnAddedAsChildCore(this);
            EventScheduler.Instance.Dispatch(new EventType.OnAddChild() { Parent = this, Child = childEntity });
        }
        private void OnRemoveChild(AEntity childEntity)
        {
            (this as IRemoveChild)?.OnRemoveChildCore(childEntity);
            (childEntity as IRemovedFromChild)?.OnRemovedFromChildCore(this);
            EventScheduler.Instance.Dispatch(new EventType.OnRemoveChild() { Parent = this, Child = childEntity });
        }
        private void OnParentChanged(AEntity previous, AEntity recent)
        {
            bool isComponent = IsComponent();
            if (isComponent && _parent && this.Enabled)
                StarLogger.LogError($"组件{this.GetType().ToString()},InstanceID: {this.InstanceId}在未被回收且已被挂载的情况下改变父级, 这一般是不被允许的");
            if (isComponent)
            {
                previous?.RemoveComponent(this as AComponent);
                recent?.AddComponent(this as AComponent);
            }
            else
            {
                previous?.RemoveChild(this);
                recent?.AddChild(this);
            }
            (this as IParentChanged)?.OnParentChangedCore(previous, recent);
        }
    }
}
