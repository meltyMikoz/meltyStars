using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D;

namespace MeltyStars
{
    public static class SpriteAtlasHelper
    {
        public static SpriteAtlas GetSpriteAtlas(string name)
        {
            return AddressableAssetScheduler.Instance.LoadAsset<SpriteAtlas>(name);
        }
    }
}
