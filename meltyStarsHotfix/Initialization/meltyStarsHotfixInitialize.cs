using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MeltyStars
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
            StarLogger.LogInfo("Initialize");
            StarLogger.LogWarning("Warning");
            StarLogger.LogError("Error");

            var g = await UpdateTest.TestTask();

            print(g);
        }

    }
}