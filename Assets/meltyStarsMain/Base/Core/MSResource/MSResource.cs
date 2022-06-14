using System.Collections;
using System.Collections.Generic;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine;
using System;
using System.Threading.Tasks;

namespace meltyStars
{
    public class MSResource
    {
        /// <summary>
        /// 封装AsyncOperationHandle
        /// </summary>
        /// <typeparam name="TObject"></typeparam>
        public class MSResourceRequestHandle<TObject> : IEquatable<MSResourceRequestHandle<TObject>> where TObject : UnityEngine.Object
        {
            private AsyncOperationHandle<TObject> asyncOperationHandle;
            public AsyncOperationHandle<TObject> AsyncOperationHandle => asyncOperationHandle;
            private MSResourceRequestHandle(AsyncOperationHandle<TObject> handle)
            {
                this.asyncOperationHandle = handle;
            }
            public bool IsDone => asyncOperationHandle.IsDone;
            public Exception OperationException => asyncOperationHandle.OperationException;
            public float PercentComplete => asyncOperationHandle.PercentComplete;
            public TObject Result => asyncOperationHandle.Result;
            public AsyncOperationStatus Status => asyncOperationHandle.Status;
            public Task<TObject> Task => asyncOperationHandle.Task;

            public event Action<MSResourceRequestHandle<TObject>> OnCompleted;
            public event Action OnCompletedTypeless;
            public event Action OnDestroyed;
            public TObject WaitForCompletion() => asyncOperationHandle.WaitForCompletion();
            public TObject WaitForCompletionTask()
            {
                Task.Wait();
                return Task.Result;
            }
            public static MSResourceRequestHandle<TObject> CreateHandle(AsyncOperationHandle<TObject> asyncOperationHandle)
            {
                return new MSResourceRequestHandle<TObject>(asyncOperationHandle);
            }
            public bool Equals(MSResourceRequestHandle<TObject> other)
            {
                return asyncOperationHandle.Equals(other.AsyncOperationHandle);
            }
        }
        public static MSResourceRequestHandle<TObject> LoadAsync<TObject>(string group, string name,
                                                        System.Action<TObject> action) where TObject : UnityEngine.Object
        {
            string path = $"{group}/{name}";
            return null;
        }
    }
}
