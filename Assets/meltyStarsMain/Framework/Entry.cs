using UnityEngine;
using MeltyStars;
using UnityEngine.U2D;

/// <summary>
/// 入口
/// </summary>
namespace SyntheticWatermelonLikeGame
{
    public class Entry : MonoBehaviour
    {
        void Awake()
        {
            DontDestroyOnLoad(this);
        }
        void OnEnable()
        {
            SpriteAtlasManager.atlasRequested += OnSpriteAtlasRequest;
        }
        void Start()
        {
            App.Instance.Awake();
        }
        void Update()
        {
            App.Instance.Update();
        }
        void LateUpdate()
        {
            App.Instance.LateUpdate();
        }
        void FixedUpdate()
        {
            App.Instance.FixedUpdate();
        }
        void OnDisable()
        {
            SpriteAtlasManager.atlasRequested -= OnSpriteAtlasRequest;
        }
        void OnDestroy()
        {
            App.Instance.Destroy(false);
        }
        void OnSpriteAtlasRequest(string name, System.Action<SpriteAtlas> callBack)
        {
            var spriteAtlas = AddressableAssetScheduler.Instance.LoadAsset<SpriteAtlas>(name);
            callBack(spriteAtlas);
        }
    }
}
