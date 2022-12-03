using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

namespace MeltyStars
{
    public enum E_RuntimeDebugType
    {
        Editor = 0,
        Release = 1
    }
    public class Initialize : MonoBehaviour
    {
        [SerializeField] E_RuntimeDebugType debugType = E_RuntimeDebugType.Editor;
        /// <summary>
        /// 在这里加载热更新Dll入口
        /// </summary>
        void Start()
        {
#if UNITY_EDITOR
            if (debugType == E_RuntimeDebugType.Editor)
                //在编辑器内直接调用Hotfix程序集
                HotFixInitialize._Instance?.Initialize();
            else
            {
                //编辑器内dll调试
                var bytes = File.ReadAllBytes($"{Application.streamingAssetsPath}/Hotfix.dll.bytes");
                var ass = System.Reflection.Assembly.Load(bytes);
                var type = ass.GetType("meltyStars.meltyStarsHotfixInitialize");
                var initialize = type.GetMethod("Init");
                initialize.Invoke(null, null);
            }
#else
            //打包后通过反射调用
            var bytes = File.ReadAllBytes($"{Application.streamingAssetsPath}/Hotfix.dll.bytes");
            var ass = System.Reflection.Assembly.Load(bytes);
            var type = ass.GetType("meltyStars.meltyStarsHotfixInitialize");
            var initialize = type.GetMethod("Init");
            initialize.Invoke(null, null);
#endif
        }
    }
}