using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MeltyStars.Hotfix
{
    public class HelloMeltyHotfix
    {
        public static void Say()
        {
            Debug.Log("Hello Melty Hotfix");
        }

        public static void Do()
        {
            GameObject g = new GameObject("Create By Hotfix");
            g.AddComponent<UpdateTest>();
        }
    }
}
