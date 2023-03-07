using UnityEngine;

namespace KuusouEngine.EngineImplement
{
    public abstract class BuiltinManagerProxy<TManager> : MonoBehaviour where TManager : class
    {
        protected TManager _manager;
        protected virtual void Awake() 
        {
            _manager = KuusouEngineEntry.GetModule<TManager>();
        }
    }
}
