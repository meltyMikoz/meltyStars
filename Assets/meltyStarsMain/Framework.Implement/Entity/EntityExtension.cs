using KuusouEngine.EngineBasic.Entity;
using System;

namespace KuusouEngine.EngineImplement.Entity
{
    public static class EntityExtension
    {
        public static TComponent AddComponent<TComponent>(this IEntity entity) where TComponent : class, IComponent
        {
            return Entry.EntityManager.AddComponent<TComponent>(entity);
        }
        public static IComponent AddComponent(this IEntity entity, Type componentType)
        {
            return Entry.EntityManager.AddComponent(entity, componentType);
        }
        public static void RemoveComponent<TComponent>(this IEntity entity) where TComponent : class, IComponent
        {
            Entry.EntityManager.RemoveComponent<TComponent>(entity);
        }
        public static void RemoveComponent(this IEntity entity, Type componentType)
        {
            Entry.EntityManager.RemoveComponent(entity, componentType);
        }
        public static TComponent GetComponent<TComponent>(this IEntity entity) where TComponent : class, IComponent
        {
            return Entry.EntityManager.GetComponent<TComponent>(entity);
        }
        public static IComponent GetComponent(this IEntity entity, Type componentType)
        {
            return Entry.EntityManager.GetComponent(entity, componentType);
        }
        public static void Register(this IEntity entity, string entityGroupName, object userData = null)
        {
            Entry.EntityManager.RegisterEntity(entity, entityGroupName, userData);
        }
        public static void Register(this IEntity entity, IEntityGroup entityGroup, object userData = null)
        {
            Entry.EntityManager.RegisterEntity(entity, entityGroup, userData);
        }
        public static void Activate(this IEntity entity, object userData = null)
        {
            Entry.EntityManager.ActivateEntity(entity, userData);
        }
        public static void Inactivate(this IEntity entity, object userData = null)
        {
            Entry.EntityManager.InactivateEntity(entity, userData);
        }
        public static void GetParent(this IEntity entity)
        {
            Entry.EntityManager.GetParentEntity(entity);
        }
    }
}
