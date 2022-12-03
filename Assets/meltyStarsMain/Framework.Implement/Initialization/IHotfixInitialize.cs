using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MeltyStars
{
    public abstract class HotFixInitialize : MonoBehaviour
    {
        public static HotFixInitialize _Instance;
        public abstract void Initialize();
    }
}