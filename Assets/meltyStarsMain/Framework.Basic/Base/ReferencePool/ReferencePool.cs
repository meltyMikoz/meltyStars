using System;
using System.Collections.Generic;

namespace KuusouEngine
{
    public static partial class ReferencePool
    {
        private static readonly Dictionary<Type, ReferenceCollection> s_referenceCollections = new Dictionary<Type, ReferenceCollection>();
        private static bool s_enableForceCheck = false;

        public static bool EnableForceCheck
        {
            get
            {
                return s_enableForceCheck;
            }
            set
            {
                s_enableForceCheck = value;
            }
        }
        public static int Count
        {
            get
            {
                return s_referenceCollections.Count;
            }
        }
        public static T Fetch<T>() where T : class, IReference, new()
        {
            return GetReferenceCollection(typeof(T)).Fetch<T>();
        }
        public static IReference Fetch(Type referenceType)
        {
            InternalCheckReferenceType(referenceType);
            return GetReferenceCollection(referenceType).Fetch();
        }
        public static void Release(IReference reference)
        {
            if (reference is null)
            {
                throw new KuusouEngineException("Reference is invalid");
            }
            Type referenceType = reference.GetType();
            InternalCheckReferenceType(referenceType);
            GetReferenceCollection(referenceType).Release(reference);
        }
        public static void ClearAll()
        {
            lock (s_referenceCollections)
            {
                foreach (KeyValuePair<Type, ReferenceCollection> referenceCollection in s_referenceCollections)
                {
                    referenceCollection.Value.RemoveAll();
                }

                s_referenceCollections.Clear();
            }
        }
        public static void Add<T>(int count) where T : class, IReference, new()
        {
            GetReferenceCollection(typeof(T)).Add<T>(count);
        }
        public static void Add(Type referenceType, int count)
        {
            InternalCheckReferenceType(referenceType);
            GetReferenceCollection(referenceType).Add(count);
        }
        public static void Remove<T>(int count) where T : class, IReference
        {
            GetReferenceCollection(typeof(T)).Remove(count);
        }
        public static void Remove(Type referenceType, int count)
        {
            InternalCheckReferenceType(referenceType);
            GetReferenceCollection(referenceType).Remove(count);
        }
        public static void RemoveAll<T>() where T : class, IReference
        {
            GetReferenceCollection(typeof(T)).RemoveAll();
        }
        public static void RemoveAll(Type referenceType)
        {
            InternalCheckReferenceType(referenceType);
            GetReferenceCollection(referenceType).RemoveAll();
        }
        private static void InternalCheckReferenceType(Type referenceType)
        {
            if (!s_enableForceCheck)
            {
                return;
            }
            if (referenceType == null)
            {
                throw new KuusouEngineException("Reference type is invalid.");
            }
            if (!referenceType.IsClass || referenceType.IsAbstract)
            {
                throw new KuusouEngineException("Reference type is not a non-abstract class type.");
            }
            if (!typeof(IReference).IsAssignableFrom(referenceType))
            {
                throw new KuusouEngineException(string.Format("Reference type '{0}' is invalid.", referenceType.FullName));
            }
        }
        private static ReferenceCollection GetReferenceCollection(Type referenceType)
        {
            if (referenceType is null)
            {
                throw new KuusouEngineException("Type is invalid");
            }
            ReferenceCollection referenceCollection = null;
            lock (s_referenceCollections)
            {
                if (!s_referenceCollections.TryGetValue(referenceType, out referenceCollection))
                {
                    referenceCollection = new ReferenceCollection(referenceType);
                    s_referenceCollections.Add(referenceType, referenceCollection);
                }
            }
            return referenceCollection;
        }
    }
}
