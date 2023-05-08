using UnityEngine;
using UnityEngine.U2D;
/// <summary>
/// 入口
/// </summary>
namespace KuusouEngine.EngineImplement
{
    public class Entry : MonoBehaviour
    {
        public static EntityManagerProxy EntityManager
        {
            get;
            private set;
        }
        public static FsmManagerProxy FsmManager 
        { 
            get; 
            private set; 
        }
        public static EventManagerProxy EventManager
        {
            get;
            private set;
        }
        void Awake()
        {
            DontDestroyOnLoad(this);
            InitBuiltinManagerProxy();
        }
        void OnEnable()
        {
            SpriteAtlasManager.atlasRequested += OnSpriteAtlasRequest;
        }
        void Update()
        {
            KuusouEngineEntry.Update(Time.deltaTime, Time.unscaledDeltaTime);
        }
        void OnDisable()
        {
            SpriteAtlasManager.atlasRequested -= OnSpriteAtlasRequest;
        }
        void OnDestroy()
        {
            KuusouEngineEntry.Shutdown();
        }
        void OnSpriteAtlasRequest(string name, System.Action<SpriteAtlas> callBack)
        {
            var spriteAtlas = AddressableAssetScheduler.Instance.LoadAsset<SpriteAtlas>(name);
            callBack(spriteAtlas);
        }
        private void InitBuiltinManagerProxy()
        { 
            EntityManager = transform.Find("Builtin/EntityManagerProxy").GetComponent<EntityManagerProxy>();
            FsmManager = transform.Find("Builtin/FsmManagerProxy").GetComponent<FsmManagerProxy>();
            EventManager = transform.Find("Builtin/EventManagerProxy").GetComponent<EventManagerProxy>();
        }
    }
}
