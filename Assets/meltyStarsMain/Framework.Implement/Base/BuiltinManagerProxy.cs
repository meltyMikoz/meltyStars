using UnityEngine;

namespace KuusouEngine.EngineImplement
{
    public abstract class BuiltinManagerProxy<TManager> : MonoBehaviour where TManager : class
    {
        private static TManager s_manager;
        protected TManager _manager
        {
            get
            {
                if (s_manager is null)
                {
                    s_manager = KuusouEngineEntry.GetModule<TManager>();
                    OnInit();
                }
                return s_manager;
            }
        }
        protected virtual void OnInit() 
        { 

        }
    }
}
