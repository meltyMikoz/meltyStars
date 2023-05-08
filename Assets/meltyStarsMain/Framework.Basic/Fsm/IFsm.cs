using System;
using System.Collections.Generic;

namespace KuusouEngine.EngineBasic.Fsm
{
    /// <summary>
    /// 有限状态机接口
    /// </summary>
    /// <typeparam name="T">有限状态机持有者类型</typeparam>
    public interface IFsm<T> where T : class
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
        /// 有限状态机持有者
        /// </summary>
        /// <value></value>
        T Owner
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
        void Start<TState>() where TState : FsmState<T>;
        void Start(Type stateType);
        bool HasState<TState>() where TState : FsmState<T>;
        bool HasState(Type stateType);
        TState GetState<TState>() where TState : FsmState<T>;
        FsmState<T> GetState(Type stateType);
        FsmState<T>[] GetAllStates();
        void GetAllStates(List<FsmState<T>> results);
        bool HasData(string name);
        TData GetData<TData>(string name) where TData : Variable;
        Variable GetData(string name);
        void SetData<TData>(string name, TData data) where TData : Variable;
        void SetData(string name, Variable data);
        bool RemoveData(string name);
    }
}
