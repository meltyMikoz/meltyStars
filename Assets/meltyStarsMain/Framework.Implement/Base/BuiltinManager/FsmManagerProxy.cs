using KuusouEngine.EngineBasic.Fsm;
using System;
using System.Collections.Generic;

namespace KuusouEngine.EngineImplement
{
    public class FsmManagerProxy : BuiltinManagerProxy<IFsmManager>
    {
        public int Count
        {
            get
            {
                return _manager.Count;
            }
        }

        public IFsm<T> CreateFsm<T>(T owner, params FsmState<T>[] states) where T : class
        {
            return _manager.CreateFsm(owner, states);
        }

        public IFsm<T> CreateFsm<T>(string name, T owner, params FsmState<T>[] states) where T : class
        {
            return _manager.CreateFsm(name, owner, states);
        }

        public IFsm<T> CreateFsm<T>(T owner, List<FsmState<T>> states) where T : class
        {
            return _manager.CreateFsm(owner, states);
        }

        public IFsm<T> CreateFsm<T>(string name, T owner, List<FsmState<T>> states) where T : class
        {
            return _manager.CreateFsm(name, owner, states);
        }

        public bool DestroyFsm<T>() where T : class
        {
            return _manager.DestroyFsm<T>();
        }

        public bool DestroyFsm(Type ownerType)
        {
            return _manager.DestroyFsm(ownerType);
        }

        public bool DestroyFsm<T>(string name) where T : class
        {
            return _manager.DestroyFsm<T>(name);
        }

        public bool DestroyFsm(Type ownerType, string name)
        {
            return _manager.DestroyFsm(ownerType, name);
        }

        public bool DestroyFsm<T>(IFsm<T> fsm) where T : class
        {
            return _manager.DestroyFsm(fsm);
        }

        public bool DestroyFsm(FsmBase fsm)
        {
            return _manager.DestroyFsm(fsm);
        }

        public FsmBase[] GetAllFsms()
        {
            return _manager.GetAllFsms();
        }

        public void GetAllFsms(List<FsmBase> results)
        {
            _manager.GetAllFsms(results);
        }

        public IFsm<T> GetFsm<T>() where T : class
        {
            return _manager.GetFsm<T>();
        }

        public FsmBase GetFsm(Type ownerType)
        {
            return _manager.GetFsm(ownerType);
        }

        public IFsm<T> GetFsm<T>(string name) where T : class
        {
            return _manager.GetFsm<T>(name);
        }

        public FsmBase GetFsm(Type ownerType, string name)
        {
            return _manager.GetFsm(ownerType, name);
        }

        public bool HasFsm<T>() where T : class
        {
            return _manager.HasFsm<T>();
        }

        public bool HasFsm(Type ownerType)
        {
            return _manager.HasFsm(ownerType);
        }

        public bool HasFsm<T>(string name) where T : class
        {
            return _manager.HasFsm<T>(name);
        }

        public bool HasFsm(Type ownerType, string name)
        {
            return _manager.HasFsm(ownerType, name);
        }
    }
}
