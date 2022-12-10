using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;


namespace KuusouEngine
{
    public class AddressableAssetScheduler : SingletonFor<AddressableAssetScheduler>
    {
        public Dictionary<string, IEnumerator> resDic = new Dictionary<string, IEnumerator>(8);
        public T LoadAsset<T>(string name)
        {
            string keyName = $"{name}_{typeof(T).Name}";
            AsyncOperationHandle<T> handle;
            if (resDic.ContainsKey(keyName))
            {
                handle = (AsyncOperationHandle<T>)resDic[keyName];
                if (handle.Status == AsyncOperationStatus.Succeeded)
                {
                    return handle.Result;
                }
                return handle.WaitForCompletion();
            }
            handle = Addressables.LoadAssetAsync<T>(name);
            resDic.Add(keyName, handle);
            return handle.WaitForCompletion();
        }
        public void LoadAssetAsync<T>(string name, Action<AsyncOperationHandle<T>> callBack)
        {
            string keyName = $"{name}_{typeof(T).Name}";

            AsyncOperationHandle<T> handle;
            //如果加载过
            if (resDic.ContainsKey(keyName))
            {
                handle = (AsyncOperationHandle<T>)resDic[keyName];
                if (handle.IsDone)
                {
                    callBack(handle);
                }
                else
                {
                    handle.Completed += handle =>
                   {
                       if (handle.Status == AsyncOperationStatus.Succeeded)
                       {
                           callBack(handle);
                       }
                   };
                }

                return;
            };

            //没有加载过
            handle = Addressables.LoadAssetAsync<T>(name);
            handle.Completed += handle =>
            {
                if (handle.Status == AsyncOperationStatus.Succeeded)
                {
                    callBack(handle);
                }
                else
                {
                    Debug.LogWarning($"{keyName}加载失败");
                    if (resDic.ContainsKey(keyName))
                        resDic.Remove(keyName);
                }
            };
            resDic.Add(keyName, handle);
        }

        public void Release<T>(string name)
        {
            string keyName = $"{name}_{typeof(T).Name}";
            if (resDic.ContainsKey(keyName))
            {
                Addressables.Release((AsyncOperationHandle<T>)resDic[keyName]);
                resDic.Remove(keyName);
            }
        }

        public void Clear()
        {
            resDic.Clear();
            AssetBundle.UnloadAllAssetBundles(true);
            Resources.UnloadUnusedAssets();
            GC.Collect();
        }
        /// <summary>
        /// 异步加载多个资源 或者 加载指定资源
        /// </summary>
        /// <param name="mode"></param>
        /// <param name="callBack"></param>
        /// <param name="key"></param>
        /// <typeparam name="T"></typeparam>
        public void LoadAssetAsync<T>(Addressables.MergeMode mode, Action<T> callBack, params string[] keys)
        {
            List<string> list = keys.ToList();
            string keyName = "";
            foreach (string key in keys)
            {
                keyName += $"{key}_";
            }
            keyName += typeof(T).Name;
            AsyncOperationHandle<IList<T>> handle;

            //如果有
            if (resDic.ContainsKey(keyName))
            {
                handle = (AsyncOperationHandle<IList<T>>)resDic[keyName];
                if (handle.IsDone)
                {
                    foreach (T item in handle.Result)
                        callBack(item);
                }
                else
                {
                    handle.Completed += handleObj =>
                    {
                        if (handleObj.Status == AsyncOperationStatus.Succeeded)
                        {
                            foreach (T item in handleObj.Result)
                                callBack(item);
                        }
                    };
                }
                return;
            };
            //不存在
            handle = Addressables.LoadAssetsAsync<T>(list, callBack, mode);
            handle.Completed += handleObj =>
            {
                if (handleObj.Status == AsyncOperationStatus.Failed)
                {
                    Debug.LogError($"加载失败,资源类型为{typeof(T).Name},资源key为{keyName}");
                    if (resDic.ContainsKey(keyName))
                        resDic.Remove(keyName);
                }
            };

            resDic.Add(keyName, handle);
        }

        public void Release<T>(params string[] keys)
        {
            List<string> list = keys.ToList();
            string keyName = "";
            foreach (string key in keys)
            {
                keyName += $"{key}_";
            }
            keyName += typeof(T).Name;

            AsyncOperationHandle<IList<T>> handle = (AsyncOperationHandle<IList<T>>)resDic[keyName];
            if (resDic.ContainsKey(keyName))
                resDic.Remove(keyName);

        }
    }
}
