using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

namespace meltyStars.Main
{
    public class Initialize : MonoBehaviour
    {
        /// <summary>
        /// 在这里加载热更新Dll入口
        /// </summary>
        void Start()
        {
#if UNITY_EDITOR
            //在编辑器内直接调用Hotfix程序集
            HotFixInitialize._Instance?.Initialize();
#else
            //打包后通过反射调用
            var bytes = File.ReadAllBytes($"{Application.streamingAssetsPath}/meltyStars.Hotfix.dll.bytes");
            var ass = System.Reflection.Assembly.Load(bytes);
            var type = ass.GetType("meltyStars.Hotfix.meltyStarsHotfixInitialize");
            var initialize = type.GetMethod("Init");
            initialize.Invoke(null, null);
#endif
        }
    }
}