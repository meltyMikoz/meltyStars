using System;
using System.Collections.Generic;
using UnityEngine;

namespace MeltyStars
{
    public partial class ObjectScheduler : SingletonFor<ObjectScheduler>
    {
        private readonly SubPoolGameObject m_SubPoolGameObject = Activator.CreateInstance(typeof(SubPoolGameObject), true) as SubPoolGameObject;
        private readonly SubPoolAnyType m_SubPoolAnyType = Activator.CreateInstance(typeof(SubPoolAnyType), true) as SubPoolAnyType;
        public GameObject GetGameObject(string namePath, string poolName = null, bool active = true, Transform parent = null)
        {
            return m_SubPoolGameObject.Get(namePath, poolName, active, parent);
        }
        public void PushGameObject(GameObject gameObject, string poolName = null)
        {
            m_SubPoolGameObject.Push(gameObject, poolName);
        }
        public T Get<T>() where T : class
        {
            return m_SubPoolAnyType.Get<T>();
        }
        public void Push<T>(T instance) where T : class
        {
            m_SubPoolAnyType.Push<T>(instance);
        }
    }
    public partial class ObjectScheduler
    {
        private class SubPoolGameObject
        {
            private class InnerPoolGameObject
            {
                private GameObject m_SourceReference;
                private readonly GameObject m_Root;
                private readonly Queue<GameObject> m_Pool = new Queue<GameObject>();
                public InnerPoolGameObject(string namePath, GameObject parent)
                {
                    m_SourceReference = AddressableAssetScheduler.Instance.LoadAsset<GameObject>(namePath);
                    m_Root = new GameObject(namePath);
                    m_Root.transform.SetParent(parent.transform);
                }
                public GameObject Get(bool active, Transform parent)
                {
                    GameObject gameObject;
                    gameObject = m_Pool.Count == 0
                               ? GameObject.Instantiate(m_SourceReference)
                               : gameObject = m_Pool.Dequeue();
                    gameObject.name = m_SourceReference.name;
                    gameObject.SetActive(active);
                    gameObject.transform.SetParent(parent);
                    return gameObject;
                }
                public void Push(GameObject gameObject)
                {
                    m_Pool.Enqueue(gameObject);
                    gameObject.SetActive(false);
                    gameObject.transform.SetParent(m_Root.transform);
                }
            }
            private readonly GameObject m_Root;
            private readonly Dictionary<string, InnerPoolGameObject> m_Pool = new Dictionary<string, InnerPoolGameObject>();
            private SubPoolGameObject() { GameObject.DontDestroyOnLoad(m_Root = new GameObject("PoolRoot")); }
            public GameObject Get(string namePath, string poolName, bool active, Transform parent)
            {
                string _poolName = namePath;
                if (!string.IsNullOrEmpty(poolName))
                    _poolName = poolName;
                if (!m_Pool.ContainsKey(_poolName))
                    m_Pool.Add(_poolName, new InnerPoolGameObject(namePath, m_Root));
                return m_Pool[_poolName].Get(active, parent);
            }
            public void Push(GameObject gameObject, string poolName)
            {
                string _poolName = gameObject.name;
                if (!string.IsNullOrEmpty(poolName))
                    _poolName = poolName;
                if (!m_Pool.ContainsKey(_poolName))
                    m_Pool.Add(_poolName, new InnerPoolGameObject(_poolName, m_Root));
                m_Pool[_poolName].Push(gameObject);
            }
        }
        private class SubPoolAnyType
        {
            private abstract class AInnerPoolAnyType { }
            private class InnerPoolAnyType<T> : AInnerPoolAnyType
            {
                private readonly Queue<T> m_Pool;
                public InnerPoolAnyType() { m_Pool = new Queue<T>(); }
                public T Get()
                {
                    if (m_Pool.Count > 0)
                        return m_Pool.Dequeue();
                    else
                        return (T)Activator.CreateInstance(typeof(T), true);
                }
                public void Push(T instance)
                {
                    m_Pool.Enqueue(instance);
                }
            }
            private readonly Dictionary<Type, AInnerPoolAnyType> m_Pool = new Dictionary<Type, AInnerPoolAnyType>();
            public T Get<T>()
            {
                Type type = typeof(T);
                if (!m_Pool.ContainsKey(type))
                    m_Pool.Add(type, new InnerPoolAnyType<T>());
                return (m_Pool[type] as InnerPoolAnyType<T>).Get();
            }
            public void Push<T>(T instance)
            {
                Type type = instance.GetType();
                if (!m_Pool.ContainsKey(type))
                    m_Pool.Add(type, new InnerPoolAnyType<T>());
                (m_Pool[type] as InnerPoolAnyType<T>).Push(instance);
            }
        }
    }
}
