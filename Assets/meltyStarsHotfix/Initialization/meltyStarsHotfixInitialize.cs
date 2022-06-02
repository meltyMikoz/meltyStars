using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using meltyStars.Main;

namespace meltyStars.Hotfix
{
    public class meltyStarsHotfixInitialize : HotFixInitialize
    {
        private void Awake()
        {
            _Instance = this;
        }
        public override void Initialize()
        {
            Init();
        }
        public static void Init()
        {
            Debug.Log($"meltyStarsHotfix 被初始化辣");
        }

    }
}