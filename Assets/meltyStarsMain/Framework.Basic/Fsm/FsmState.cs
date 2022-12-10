using System;

namespace KuusouEngine.EngineBasic.Fsm
{
    /// <summary>
    /// 有限状态机状态
    /// </summary>
    /// <typeparam name="T">有限状态机持有者类型</typeparam>
    public class FsmState<T> where T : class
    {
        protected internal void OnInit(IFsm<T> fsm)
        {

        }
    }
}
