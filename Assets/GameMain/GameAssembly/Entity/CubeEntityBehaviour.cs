using KuusouEngine.EngineBasic.Entity;
using KuusouEngine.EngineImplement;
using KuusouEngine.EngineImplement.Entity;

namespace GameMainTest.Entity
{
    [BindWithSystem(typeof(CubeEntitySystem))]
    public class CubeEntityBehaviour : EntityBehaviour
    {
        public float Time;
        protected virtual void Start()
        {
            Entry.EntityManager.AddEntityGroup("CubeEntity", EntityBehaviourGroupHelper.Instance);
            this.Register("CubeEntity", 123123);
        }
    }
}