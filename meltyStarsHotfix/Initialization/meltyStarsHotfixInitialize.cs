using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace meltyStars
{
    public class meltyStarsHotfixInitialize : HotFixInitialize
    {
        private void OnEnable()
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
        public async static void Init()
        {
            MSLogger.LogInfo("Initialize");
            MSLogger.LogWarning("Warning");
            MSLogger.LogError("Error");

            var g = await UpdateTest.TestTask();

            print(g);
        }

    }
}