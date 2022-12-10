using System;
using System.Collections.Generic;

namespace KuusouEngine
{
    public static partial class ReferencePool
    {
        private sealed class ReferenceCollection
        {
            private readonly Queue<IReference> _references;
            private readonly Type _referenceType;
            private int _referenceUsingCount;
            private int _referenceFetchCount;
            private int _referenceReleaseCount;
            private int _referenceAddCount;
            private int _referenceRemoveCount;
            public ReferenceCollection(Type referenceType)
            {
                this._references = new Queue<IReference>();
                this._referenceType = referenceType;
                this._referenceUsingCount = 0;
                this._referenceFetchCount = 0;
                this._referenceReleaseCount = 0;
                this._referenceAddCount = 0;
                this._referenceRemoveCount = 0;
            }
            public Type ReferenceType
            {
                get
                {
                    return this._referenceType;
                }
            }
            public int ReferenceUnusedCount
            {
                get
                {
                    return this._references.Count;
                }
            }
            public int ReferenceUsingCount
            {
                get
                {
                    return this._referenceUsingCount;
                }
            }
            public int ReferenceFetchCount
            {
                get
                {
                    return this._referenceFetchCount;
                }
            }
            public int ReferenceReleaseCount
            {
                get
                {
                    return this._referenceReleaseCount;
                }
            }
            public int ReferenceAddCount
            {
                get
                {
                    return this._referenceAddCount;
                }
            }
            public int ReferenceRemoveCount
            {
                get
                {
                    return this._referenceRemoveCount;
                }
            }
            public T Fetch<T>() where T : class, IReference, new()
            {
                if (typeof(T) != this._referenceType)
                {
                    throw new KuusouEngineException("Reference type is invalid");
                }
                this._referenceUsingCount++;
                this._referenceFetchCount++;
                lock (this._references)
                {
                    if (this._references.Count > 0)
                    {
                        return this._references.Dequeue() as T;
                    }
                }
                this._referenceAddCount++;
                return new T();
            }
            public IReference Fetch()
            {
                this._referenceUsingCount++;
                this._referenceFetchCount++;
                lock (this._references)
                {
                    if (this._references.Count > 0)
                    {
                        return this._references.Dequeue();
                    }
                }
                this._referenceAddCount++;
                return Activator.CreateInstance(this._referenceType) as IReference;
            }
            public void Release(IReference reference)
            {
                reference.Clear();
                lock (this._references)
                {
                    if (s_enableForceCheck && this._references.Contains(reference))
                    {
                        throw new KuusouEngineException("Current reference has been released");
                    }
                    this._references.Enqueue(reference);
                }
                this._referenceReleaseCount++;
                this._referenceUsingCount--;
            }
            public void Add<T>(int count) where T : class, IReference, new()
            {
                if (typeof(T) != this._referenceType)
                {
                    throw new KuusouEngineException("Reference type is invalid");
                }
                lock (this._references)
                {
                    this._referenceAddCount += count;
                    while (count-- > 0)
                    {
                        this._references.Enqueue(new T());
                    }
                }
            }
            public void Add(int count)
            {
                lock (this._references)
                {
                    this._referenceAddCount += count;
                    while (count-- > 0)
                    {
                        this._references.Enqueue(Activator.CreateInstance(this._referenceType) as IReference);
                    }
                }
            }
            public void Remove(int count)
            {
                lock (this._references)
                {
                    if (count > this._references.Count)
                    {
                        count = this._references.Count;
                    }
                    this._referenceRemoveCount += count;
                    while (count-- > 0)
                    {
                        this._references.Dequeue();
                    }
                }
            }
            public void RemoveAll()
            {
                lock (this._references)
                {
                    this._referenceRemoveCount += this._references.Count;
                    this._references.Clear();
                }
            }
        }
    }
}
