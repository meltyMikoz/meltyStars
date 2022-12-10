using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using System.IO;

namespace KuusouEngine.EngineBasic
{
    public class HelloMelty : MonoBehaviour
    {
        // Start is called before the first frame update
        void Start()
        {
            TextAsset hotfixBytes = Resources.Load("meltyStars.Hotfix.dll") as TextAsset;
            var ass = System.Reflection.Assembly.Load(hotfixBytes.bytes);

            var type = ass.GetType("meltyStars.Hotfix.HelloMeltyHotfix");
            var me = type.GetMethod("Do");
            var me2 = type.GetMethod("Say");
            me.Invoke(null, null);
            me2.Invoke(null, null);
        }
    }
}
