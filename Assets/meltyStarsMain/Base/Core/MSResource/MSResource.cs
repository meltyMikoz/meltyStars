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
        public class MSResourceRequestHandle<TObject> : IEquatable<MSResourceRequestHandle<TObject>>, IDisposable where TObject : UnityEngine.Object
        {
            private AsyncOperationHandle<TObject> asyncOperationHandle;
            public AsyncOperationHandle<TObject> AsyncOperationHandle => asyncOperationHandle;
            private MSResourceRequestHandle(AsyncOperationHandle<TObject> asyncOperationHandle)
            {
                this.asyncOperationHandle = asyncOperationHandle;
                asyncOperationHandle.Completed += _ =>
                {
                    OnCompleted?.Invoke(this.Result);
                    OnCompletedHandle?.Invoke(this);
                };
            }
            public bool IsDone => asyncOperationHandle.IsDone;
            public Exception OperationException => asyncOperationHandle.OperationException;
            public float PercentComplete => asyncOperationHandle.PercentComplete;
            public TObject Result => asyncOperationHandle.Result;
            public AsyncOperationStatus Status => asyncOperationHandle.Status;
            public Task<TObject> Task => asyncOperationHandle.Task;
            public event Action<TObject> OnCompleted;
            public event Action<MSResourceRequestHandle<TObject>> OnCompletedHandle;
            public TObject WaitForCompletion() => asyncOperationHandle.WaitForCompletion();
            public static MSResourceRequestHandle<TObject> CreateHandle(AsyncOperationHandle<TObject> asyncOperationHandle)
            {
                return new MSResourceRequestHandle<TObject>(asyncOperationHandle);
            }
            public bool Equals(MSResourceRequestHandle<TObject> other)
            {
                return asyncOperationHandle.Equals(other.AsyncOperationHandle);
            }
            public MSResourceRequestHandle<TObject> Subscribe(Action<TObject> onCompleted, Action<MSResourceRequestHandle<TObject>> onCompletedHandle = null)
            {
                this.OnCompleted = onCompleted;
                if (onCompletedHandle != null)
                    this.OnCompletedHandle = onCompletedHandle;
                return this;
            }
            public void Dispose()
            {

            }
        }
        public static TObject Load<TObject>(string path) where TObject : UnityEngine.Object
        {
            var msHandle = MSResourceRequestHandle<TObject>.CreateHandle(Addressables.LoadAssetAsync<TObject>(path));
            TObject result = msHandle.WaitForCompletion();
            return result;
        }
        public static TObject Load<TObject>(params string[] paths) where TObject : UnityEngine.Object
        {
            string path = string.Concat(string.Join("/", paths));
            var msHandle = MSResourceRequestHandle<TObject>.CreateHandle(Addressables.LoadAssetAsync<TObject>(path));
            TObject result = msHandle.WaitForCompletion();
            return result;
        }
        public static MSResourceRequestHandle<TObject> LoadAsync<TObject>(string path) where TObject : UnityEngine.Object
        {
            return null;
        }
    }
}
