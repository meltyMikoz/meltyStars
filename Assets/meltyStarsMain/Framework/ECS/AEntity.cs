using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MeltyStars
{
    public abstract class AEntity : IDisposable
    {
        private readonly Guid m_InstanceID = Guid.NewGuid();
        public string InstanceID => m_InstanceID.ToString();
        private AEntity m_Parent;
        public AEntity Parent
        {
            get
            {
                return m_Parent;
            }
            set
            {
                m_Parent = value;
                OnParentChanged(value);
            }
        }
        protected readonly Dictionary<Type, Dictionary<string, AEntity>> m_Children = new Dictionary<Type, Dictionary<string, AEntity>>();
        protected readonly Dictionary<Type, AComponent> m_Components = new Dictionary<Type, AComponent>();
        public bool IsAwake { get; protected set; }
        public bool IsDestroy { get; protected set; }
        #region Component
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
            AddComponentCore(componentType, component);
        }
        public void AddComponent<TComponent>(TComponent component) where TComponent : AComponent
        {
            Type componentType = typeof(TComponent);
            AddComponentCore(componentType, component);
        }
        private AComponent AddComponentCore(Type componentType)
        {
            // if (!m_Components.ContainsKey(componentType))
            // {
            //     m_Components.Add(componentType, new Dictionary<Guid, AComponent>());
            // }
            if (m_Components.ContainsKey(componentType)) return null;
            AComponent component = Activator.CreateInstance(componentType, true) as AComponent;
            m_Components.Add(componentType, component);
            //触发OnAddComponent
            this.OnAddComponent(component);
            return component;
        }
        private void AddComponentCore(Type componentType, AComponent component)
        {
            if (m_Components.ContainsKey(componentType)) return;
            m_Components.Add(componentType, component);
            this.OnAddComponent(component);
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
            if (!m_Components.ContainsKey(componentType)) return;
            AComponent component = m_Components[componentType];
            m_Components.Remove(componentType);
            //触发OnRemoveComponent
            this.OnRemoveComponent(component);
        }
        public bool HasComponent<TComponent>()
        {
            return m_Components.ContainsKey(typeof(TComponent));
        }
        public bool HasComponent(Type componentType)
        {
            return m_Components.ContainsKey(componentType);
        }
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
            if (!m_Components.ContainsKey(componentType)) return null;
            return m_Components[componentType];
        }
        /// <summary>
        /// 是否为组件
        /// </summary>
        /// <returns></returns>
        public bool IsComponent()
        {
            return typeof(AComponent).IsAssignableFrom(this.GetType());
        }
        #endregion
        #region Child
        /// <summary>
        /// 添加Child
        /// </summary>
        /// <param name="childEntity"></param>
        /// <typeparam name="TChildType"></typeparam>
        public void AddChild<TChildType>(TChildType childEntity) where TChildType : AEntity
        {
            Type childType = typeof(TChildType);
            if (typeof(AComponent).IsAssignableFrom(childType))
            {
                Debug.LogError($"不能添加Component为Child -> {childType}");
                return;
            }
            AddChildCore(childEntity, childType);
        }
        /// <summary>
        /// 尝试创建并添加Child
        /// </summary>
        /// <param name="fromPool"></param>
        /// <typeparam name="TChildType"></typeparam>
        /// <returns></returns>
        public TChildType AddChild<TChildType>(bool fromPool = false) where TChildType : AEntity
        {
            Type childType = typeof(TChildType);
            TChildType childEntity;
            if (typeof(AComponent).IsAssignableFrom(childType))
            {
                Debug.LogError($"不能添加Component为Child -> {childType}");
                return null;
            }
            if (fromPool)
                childEntity = ObjectScheduler.Instance.Get<TChildType>();
            else
                childEntity = Activator.CreateInstance<TChildType>();
            AddChildCore(childEntity, childType);
            return childEntity;

        }
        private void AddChildCore(AEntity childEntity, Type childType)
        {
            if (!m_Children.ContainsKey(childType))
                m_Children.Add(childType, new Dictionary<string, AEntity>());
            m_Children[childType].Add(childEntity.InstanceID, childEntity);
            //触发OnAddChild
            this.OnAddChild(childEntity);
        }
        /// <summary>
        /// 移除第一个对应类型的Child
        /// </summary>
        /// <typeparam name="TChildType"></typeparam>
        public void RemoveFirstChildOf<TChildType>() where TChildType : AEntity
        {
            Type childType = typeof(TChildType);
            if (!m_Children.ContainsKey(childType))
            {
                Debug.LogError($"[{this.GetType()}] InstanceID : {InstanceID} 没有类型为{childType}的子物体表，移除失败");
                return;
            }
            TChildType childEntity = m_Children[childType].ElementAt(0).Value as TChildType;
            m_Children[childType].Remove(m_Children[childType].ElementAt(0).Key);
            //触发OnRemoveChild
            this.OnRemoveChild(childEntity);
            if (m_Children[childType].Count == 0)
                m_Children.Remove(childType);
        }
        /// <summary>
        /// 根据实例移除Child
        /// </summary>
        /// <param name="childEntity"></param>
        /// <typeparam name="TChildType"></typeparam>
        public void RemoveChild<TChildType>(AEntity childEntity) where TChildType : AEntity
        {
            Type childType = typeof(TChildType);
            RemoveChildCore(childType, childEntity);
        }
        public void RemoveChild(Type childType, AEntity childEntity)
        {
            RemoveChildCore(childType, childEntity);
        }
        private void RemoveChildCore(Type childType, AEntity childEntity)
        {
            if (!m_Children.ContainsKey(childType))
            {
                Debug.LogError($"{this.GetType()}] InstanceID : {InstanceID} 没有类型为{childType}的子物体表，移除失败");
                return;
            }
            if (!m_Children[childType].ContainsKey(childEntity.InstanceID))
            {
                Debug.Log($"[{this.GetType()}] InstanceID : {InstanceID} 没有类型为{childType} InstanceID : {childEntity.InstanceID} 的子物体");
                return;
            }
            m_Children[childType].Remove(childEntity.InstanceID);
            //触发OnRemoveChild
            this.OnRemoveChild(childEntity);
            if (m_Children[childType].Count == 0)
                m_Children.Remove(childType);
        }
        public void RemoveChildById(string instanceID)
        {
            AEntity child = default(AEntity);
            if (child = GetChildById(instanceID))
                this.RemoveChild(child.GetType(), child);
        }
        public void RemoveChildById<TChildType>(string instanceID) where TChildType : AEntity
        {
            TChildType child = default(TChildType);
            if (child = GetChildById<TChildType>(instanceID))
                this.RemoveChild<TChildType>(child);
        }
        public void RemoveChildById(Type childType, string instanceID)
        {
            AEntity child = default(AEntity);
            if (child = GetChildById(childType, instanceID))
                this.RemoveChild(childType, child);
        }
        public TChildType GetFirstChildOf<TChildType>() where TChildType : AEntity
        {
            Type childType = typeof(TChildType);
            if (typeof(AComponent).IsAssignableFrom(childType))
            {
                Debug.LogError($"不能获取Component类型 -> {childType}");
                return null;
            }
            if (!m_Children.ContainsKey(childType))
            {
                Debug.LogError($"[{this.GetType()}] InstanceID : {InstanceID} 没有类型为{childType}的子物体表，获取失败");
                return null;
            }
            return m_Children[childType].ElementAt(0).Value as TChildType;
        }
        public AEntity GetChildById(string instanceID)
        {
            AEntity entity = default(AEntity);
            foreach (var kv1 in m_Children)
            {
                foreach (var kv2 in kv1.Value)
                {
                    if (kv2.Key == instanceID)
                        entity = kv2.Value;
                }
            }
            return entity;
        }
        public TChildType GetChildById<TChildType>(string instanceID) where TChildType : AEntity
        {
            return GetChildByIdCore(typeof(TChildType), instanceID) as TChildType;
        }
        public AEntity GetChildById(Type childType, string instanceID)
        {
            return GetChildByIdCore(childType, instanceID);
        }
        private AEntity GetChildByIdCore(Type childType, string instanceID)
        {
            AEntity entity = default(AEntity);
            if (!m_Children.ContainsKey(childType))
            {
                Debug.LogError($"[{this.GetType()}] InstanceID : {InstanceID} 没有类型为{childType}的子物体表，不能获取");
                return entity;
            }
            if (m_Children[childType].ContainsKey(instanceID))
                entity = m_Children[childType][instanceID];
            if (!entity)
                Debug.LogError($"[{this.GetType()}] InstanceID : {InstanceID} 没有类型为{childType} InstanceID : {instanceID} 的子物体");
            return entity;
        }
        #endregion
        private void OnAwake()
        {
            if (!IsAwake)
                OnAwakeCore();
            IsAwake = true;
            IsDestroy = false;
            foreach (var component in m_Components)
            {
                component.Value.Awake();
            }
            foreach (var kv1 in m_Children)
            {
                foreach (var kv2 in kv1.Value)
                {
                    kv2.Value.Awake();
                }
            }
        }
        private void OnUpdate()
        {
            OnUpdateCore();
            foreach (var component in m_Components)
            {
                component.Value.Update();
            }
            foreach (var kv1 in m_Children)
            {
                foreach (var kv2 in kv1.Value)
                {
                    kv2.Value.Update();
                }
            }
        }
        private void OnFixedUpdate()
        {
            OnFixedUpdateCore();
            foreach (var component in m_Components)
            {
                component.Value.FixedUpdate();
            }
            foreach (var kv1 in m_Children)
            {
                foreach (var kv2 in kv1.Value)
                {
                    kv2.Value.FixedUpdate();
                }
            }
        }
        private void OnLateUpdate()
        {
            OnLateUpdateCore();
            foreach (var component in m_Components)
            {
                component.Value.LateUpdate();
            }
            foreach (var kv1 in m_Children)
            {
                foreach (var kv2 in kv1.Value)
                {
                    kv2.Value.LateUpdate();
                }
            }
        }
        private void OnDestroy(bool destroyChildren, bool pushIntoPool)
        {
            if (!IsDestroy)
                OnDestroyCore();
            IsDestroy = true;
            IsAwake = false;
            while (m_Components.Count > 0)
            {
                m_Components.ElementAt(0).Value.Destroy();
            }
            if (destroyChildren)
                while (m_Children.Count > 0)
                {
                    while (m_Children.ElementAt(0).Value.Count > 0)
                    {
                        m_Children.ElementAt(0).Value
                                  .ElementAt(0).Value
                                  .Destroy(destroyChildren);
                    }
                }
            if (Parent)
            {
                if (IsComponent())
                    Parent.RemoveComponent(this.GetType());
                else
                    Parent.RemoveChildById(this.InstanceID);
            }
            //压入对象池
            if (IsComponent()) return;
            if (pushIntoPool)
                ObjectScheduler.Instance.Push<AEntity>(this);
        }
        private void OnAddComponent(AComponent component)
        {
            component.Parent = this;
            OnAddComponentCore(component);
            component.OnAddedAsComponent(this);
            EventScheduler.Instance.Dispatch(new EventType.OnAddComponent() { Entity = this, Component = component });
        }
        private void OnRemoveComponent(AComponent component)
        {
            component.Parent = null;
            OnRemoveComponentCore(component);
            component.OnRemovedFromComponent(this);
            component.Destroy();
            EventScheduler.Instance.Dispatch(new EventType.OnRemoveComponent() { Entity = this, Component = component });
        }
        private void OnAddChild(AEntity child)
        {
            child.Parent = this;
            OnAddChildCore(child);
            child.OnAddedAsChildCore(this);
            EventScheduler.Instance.Dispatch(new EventType.OnAddChild() { Parent = this, Child = child });
        }
        private void OnRemoveChild(AEntity child)
        {
            child.Parent = null;
            OnRemoveChildCore(child);
            child.OnRemovedFromChildCore(this);
            EventScheduler.Instance.Dispatch(new EventType.OnRemoveChild() { Parent = this, Child = child });
        }
        protected virtual void OnAwakeCore() { }
        protected virtual void OnUpdateCore() { }
        protected virtual void OnFixedUpdateCore() { }
        protected virtual void OnLateUpdateCore() { }
        protected virtual void OnDestroyCore() { }
        protected virtual void OnAddComponentCore(AComponent component) { }
        protected virtual void OnRemoveComponentCore(AComponent component) { }
        protected virtual void OnAddChildCore(AEntity child) { }
        protected virtual void OnAddedAsChildCore(AEntity parent) { }
        protected virtual void OnRemoveChildCore(AEntity child) { }
        protected virtual void OnRemovedFromChildCore(AEntity parent) { }
        protected virtual void OnParentChanged(AEntity parent) { }

        public void Awake() { OnAwake(); }
        public void Update() { OnUpdate(); }
        public void FixedUpdate() { OnFixedUpdate(); }
        public void LateUpdate() { OnLateUpdate(); }
        public void Destroy(bool destroyChildren = true, bool pushIntoPool = true) { OnDestroy(destroyChildren, pushIntoPool); }

        #region Dispose
        private bool disposedValue;
        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    // TODO: 释放托管状态(托管对象)
                }

                // TODO: 释放未托管的资源(未托管的对象)并替代终结器
                // TODO: 将大型字段设置为 null
                disposedValue = true;
            }
        }

        // // TODO: 仅当“Dispose(bool disposing)”拥有用于释放未托管资源的代码时才替代终结器
        // ~FrameworkBase()
        // {
        //     // 不要更改此代码。请将清理代码放入“Dispose(bool disposing)”方法中
        //     Dispose(disposing: false);
        // }

        public void Dispose()
        {
            // 不要更改此代码。请将清理代码放入“Dispose(bool disposing)”方法中
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }
        #endregion

        public static implicit operator bool(AEntity entity) => !object.ReferenceEquals(entity, null);
    }
}
