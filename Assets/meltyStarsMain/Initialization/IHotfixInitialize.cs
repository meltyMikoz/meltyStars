using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace meltyStars
{
    public abstract class HotFixInitialize : MonoBehaviour
    {
        public static HotFixInitialize _Instance;
        public abstract void Initialize();
    }
}