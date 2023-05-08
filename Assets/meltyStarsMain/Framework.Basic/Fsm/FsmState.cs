using System;

namespace KuusouEngine.EngineBasic.Fsm
{
    /// <summary>
    /// 有限状态机状态
    /// </summary>
    /// <typeparam name="T">有限状态机持有者类型</typeparam>
    public abstract class FsmState<T> where T : class
    {
        protected internal abstract void OnInit(IFsm<T> fsm);
        protected internal abstract void OnEnter(IFsm<T> fsm);
        protected internal abstract void OnUpdate(IFsm<T> fsm, float elapseFrequency, float elapseFrequencyReally);
        protected internal abstract void OnLeave(IFsm<T> fsm, bool isShutdown);
        protected internal abstract void OnDestroy(IFsm<T> fsm);
        protected void SwitchToState<TState>(IFsm<T> fsm) where TState : FsmState<T>
        {
            Fsm<T> fsmImplement = (Fsm<T>)fsm;
            if (fsmImplement == null)
            {
                throw new KuusouEngineException("Fsm is invalid.");
            }

            fsmImplement.SwitchToState<TState>();
        }
        protected void SwitchToState(IFsm<T> fsm, Type stateType)
        {
            Fsm<T> fsmImplement = (Fsm<T>)fsm;
            if (fsmImplement == null)
            {
                throw new KuusouEngineException("Fsm is invalid.");
            }

            if (stateType == null)
            {
                throw new KuusouEngineException("State type is invalid.");
            }

            if (!typeof(FsmState<T>).IsAssignableFrom(stateType))
            {
                throw new KuusouEngineException($"State type '{stateType.FullName}' is invalid.");
            }

            fsmImplement.SwitchToState(stateType);
        }
    }
}
