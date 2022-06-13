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
            private AsyncOperationHandle<TObject> handle;
            public AsyncOperationHandle<TObject> Handle => handle;
            private MSResourceRequestHandle(AsyncOperationHandle<TObject> handle)
            {
                this.handle = handle;
            }
            public string DebugName => handle.DebugName;
            public bool IsDone => handle.IsDone;
            public Exception OperationException => handle.OperationException;
            public float PercentComplete => handle.PercentComplete;
            public TObject Result => handle.Result;
            public AsyncOperationStatus Status => handle.Status;
            public Task<TObject> Task => handle.Task;

            public DownloadStatus GetDownloadStatus() => handle.GetDownloadStatus();
            public override int GetHashCode() => handle.GetHashCode();
            public bool IsValid() => handle.IsValid();
            public TObject WaitForCompletion() => handle.WaitForCompletion();
            public static MSResourceRequestHandle<TObject> CreateMSResourceRequestHandle(AsyncOperationHandle<TObject> asyncOperationHandle)
            {
                return new MSResourceRequestHandle<TObject>(asyncOperationHandle);
            }
            public bool Equals(MSResourceRequestHandle<TObject> other)
            {
                return handle.Equals(other.Handle);
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
