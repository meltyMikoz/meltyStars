using System.Collections;
using System.Collections.Generic;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine;
using System;
using System.Threading.Tasks;

namespace KuusouEngine
{
    public class MSResource
    {
        public static string ResPath = "Assets/ResBundles/";
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
