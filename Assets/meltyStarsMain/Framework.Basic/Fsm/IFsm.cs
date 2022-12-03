using System;

namespace MeltyStars.FrameworkBasic.Fsm
{
    /// <summary>
    /// 有限状态机接口
    /// </summary>
    /// <typeparam name="T">有限状态机持有者类型</typeparam>
    public class IFsm<T> where T : class
    {
        /// <summary>
        /// 有限状态机名称
        /// </summary>
        /// <value></value>
        string Name
        {
            get;
        }
        /// <summary>
        /// 有限状态机持有者类型
        /// </summary>
        /// <value></value>
        T OwnerType
        {
            get;
        }
        /// <summary>
        /// 有限状态机状态数量
        /// </summary>
        /// <value></value>
        int StateCount
        {
            get;
        }
        /// <summary>
        /// 有限状态机是否正在运行
        /// </summary>
        /// <value></value>
        bool IsRunning
        {
            get;
        }
        /// <summary>
        /// 有限状态机是否被销毁
        /// </summary>
        /// <value></value>
        bool IsDestroyed
        {
            get;
        }
        /// <summary>
        /// 有限状态机当前状态
        /// </summary>
        /// <value></value>
        FsmState<T> CurrentState
        {
            get;
        }
        /// <summary>
        /// 有限状态机当前状态保持时间
        /// </summary>
        /// <value></value>
        float CurrentStateTime
        {
            get;
        }
    }
}
