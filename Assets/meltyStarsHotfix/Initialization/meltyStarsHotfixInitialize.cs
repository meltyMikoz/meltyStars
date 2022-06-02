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
        /// <summary>
        /// 请在这里完成初始化
        /// </summary>
        public static void Init()
        {
            GameObject.Find("Text").GetComponent<UnityEngine.UI.Text>().text = "After";
        }

    }
}