using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

namespace meltyStars.Main
{
    public class HelloMelty : MonoBehaviour
    {
        // Start is called before the first frame update
        void Start()
        {
            var dlls = Directory.GetFiles(Application.streamingAssetsPath);
            foreach (var dll in dlls)
            {
                print(dll);
            }
            //TextAsset hotfixDll = 
        }

        // Update is called once per frame
        void Update()
        {

        }
    }
}
