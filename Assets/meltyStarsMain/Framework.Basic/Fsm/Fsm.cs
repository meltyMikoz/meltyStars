using System;
using System.Collections.Generic;
using static UnityEditor.VersionControl.Asset;

namespace KuusouEngine.EngineBasic.Fsm
{
    internal sealed class Fsm<T> : FsmBase, IFsm<T>, IReference where T : class
    {
        private T _owner;
        private readonly Dictionary<Type, FsmState<T>> _states;
        private Dictionary<string, Variable> _datas;
        private FsmState<T> _currentState;
        private float _currentStateTime;
        private bool _isDestroyed;
        public Fsm()
        {
            _owner = null;
            _states = new Dictionary<Type, FsmState<T>>();
            _datas = null;
            _currentState = null;
            _currentStateTime = 0;
            _isDestroyed = true;
        }
        public override Type OwnerType
        {
            get
            {
                return typeof(T);
            }
        }

        public override int StateCount
        {
            get
            {
                return _states.Count;
            }
        }

        public override bool IsRunning
        {
            get
            {
                return _currentState != null;
            }
        }

        public override bool IsDestroyed
        {
            get
            {
                return _isDestroyed;
            }
        }

        public override string CurrentStateName
        {
            get
            {
                return _currentState is null ? null : _currentState.GetType().FullName;
            }
        }

        public override float CurrentStateTime
        {
            get
            {
                return _currentStateTime;
            }
        }

        public T Owner
        {
            get
            {
                return _owner;
            }
        }

        public FsmState<T> CurrentState
        {
            get
            {
                return _currentState;
            }
        }

        public static Fsm<T> Create(string name, T owner, params FsmState<T>[] states)
        {
            if (owner == null)
            {
                throw new KuusouEngineException("Fsm owner is invalid.");
            }

            if (states == null || states.Length < 1)
            {
                throw new KuusouEngineException("Fsm states is invalid.");
            }

            Fsm<T> fsm = ReferencePool.Fetch<Fsm<T>>();
            fsm.Name = name;
            fsm._owner = owner;
            fsm._isDestroyed = false;
            foreach (FsmState<T> state in states)
            {
                if (state == null)
                {
                    throw new KuusouEngineException("Fsm states is invalid.");
                }

                Type stateType = state.GetType();
                if (fsm._states.ContainsKey(stateType))
                {
                    throw new KuusouEngineException($"Fsm '{new TypeNamePair(typeof(T), name)}' state '{stateType.FullName}' is already exist.");
                }

                fsm._states.Add(stateType, state);
                state.OnInit(fsm);
            }

            return fsm;
        }

        public static Fsm<T> Create(string name, T owner, List<FsmState<T>> states)
        {
            if (owner == null)
            {
                throw new KuusouEngineException("Fsm owner is invalid.");
            }

            if (states == null || states.Count < 1)
            {
                throw new KuusouEngineException("Fsm states is invalid.");
            }

            Fsm<T> fsm = ReferencePool.Fetch<Fsm<T>>();
            fsm.Name = name;
            fsm._owner = owner;
            fsm._isDestroyed = false;
            foreach (FsmState<T> state in states)
            {
                if (state == null)
                {
                    throw new KuusouEngineException("Fsm states is invalid.");
                }

                Type stateType = state.GetType();
                if (fsm._states.ContainsKey(stateType))
                {
                    throw new KuusouEngineException($"Fsm '{new TypeNamePair(typeof(T), name)}' state '{stateType.FullName}' is already exist.");
                }

                fsm._states.Add(stateType, state);
                state.OnInit(fsm);
            }

            return fsm;
        }

        public void Clear()
        {
            if (_currentState != null)
            {
                _currentState.OnLeave(this, true);
            }

            foreach (KeyValuePair<Type, FsmState<T>> state in _states)
            {
                state.Value.OnDestroy(this);
            }

            Name = null;
            _owner = null;
            _states.Clear();

            if (_datas != null)
            {
                foreach (KeyValuePair<string, Variable> data in _datas)
                {
                    if (data.Value == null)
                    {
                        continue;
                    }

                    ReferencePool.Release(data.Value);
                }

                _datas.Clear();
            }

            _currentState = null;
            _currentStateTime = 0f;
            _isDestroyed = true;
        }

        public FsmState<T>[] GetAllStates()
        {
            int index = 0;
            FsmState<T>[] results = new FsmState<T>[_states.Count];
            foreach (KeyValuePair<Type, FsmState<T>> state in _states)
            {
                results[index++] = state.Value;
            }

            return results;
        }

        public void GetAllStates(List<FsmState<T>> results)
        {
            if (results == null)
            {
                throw new KuusouEngineException("Results is invalid.");
            }

            results.Clear();
            foreach (KeyValuePair<Type, FsmState<T>> state in _states)
            {
                results.Add(state.Value);
            }
        }

        public TState GetState<TState>() where TState : FsmState<T>
        {
            FsmState<T> state = null;
            if (_states.TryGetValue(typeof(TState), out state))
            {
                return (TState)state;
            }

            return null;
        }

        public FsmState<T> GetState(Type stateType)
        {
            if (stateType == null)
            {
                throw new KuusouEngineException("State type is invalid.");
            }

            if (!typeof(FsmState<T>).IsAssignableFrom(stateType))
            {
                throw new KuusouEngineException($"State type '{stateType.FullName}' is invalid.");
            }

            FsmState<T> state = null;
            if (_states.TryGetValue(stateType, out state))
            {
                return state;
            }

            return null;
        }

        public bool HasState<TState>() where TState : FsmState<T>
        {
            return _states.ContainsKey(typeof(TState));
        }

        public bool HasState(Type stateType)
        {
            if (stateType is null)
            {
                throw new KuusouEngineException("State type is invalid.");
            }

            if (!typeof(FsmState<T>).IsAssignableFrom(stateType))
            {
                throw new KuusouEngineException($"State type '{stateType.FullName}' is invalid.");
            }

            return _states.ContainsKey(stateType);
        }

        public bool RemoveData(string name)
        {
            if (string.IsNullOrEmpty(name))
            {
                throw new KuusouEngineException("Data name is invalid.");
            }

            if (_datas is null)
            {
                return false;
            }

            Variable oldData = GetData(name);
            if (oldData != null)
            {
                ReferencePool.Release(oldData);
            }

            return _datas.Remove(name);
        }

        public void SetData<TData>(string name, TData data) where TData : Variable
        {
            SetData(name, data as Variable);
        }

        public void SetData(string name, Variable data)
        {
            if (string.IsNullOrEmpty(name))
            {
                throw new KuusouEngineException("Data name is invalid.");
            }

            if (_datas == null)
            {
                _datas = new Dictionary<string, Variable>(StringComparer.Ordinal);
            }

            Variable oldData = GetData(name);
            if (oldData != null)
            {
                ReferencePool.Release(oldData);
            }

            _datas[name] = data;
        }

        public TData GetData<TData>(string name) where TData : Variable
        {
            return GetData(name) as TData;
        }

        public Variable GetData(string name)
        {
            if (string.IsNullOrEmpty(name))
            {
                throw new KuusouEngineException("Data name is invalid.");
            }

            if (_datas == null)
            {
                return null;
            }

            Variable data = null;
            if (_datas.TryGetValue(name, out data))
            {
                return data;
            }

            return null;
        }


        public bool HasData(string name)
        {
            if (string.IsNullOrEmpty(name))
            {
                throw new KuusouEngineException("Data name is invalid.");
            }

            if (_datas == null)
            {
                return false;
            }

            return _datas.ContainsKey(name);
        }

        public void Start<TState>() where TState : FsmState<T>
        {
            if (IsRunning)
            {
                throw new KuusouEngineException("Fsm is already running");
            }

            FsmState<T> state = GetState<TState>();
            if (state is null)
            {
                throw new KuusouEngineException($"The fsm '{new TypeNamePair(typeof(T), Name)}' does not exist state '{typeof(TState).Name}'");
            }

            _currentStateTime = 0f;
            _currentState = state;
            _currentState.OnEnter(this);
        }

        public void Start(Type stateType)
        {
            if (IsRunning)
            {
                throw new KuusouEngineException("FSM is running, can not start again.");
            }

            if (stateType == null)
            {
                throw new KuusouEngineException("State type is invalid.");
            }

            if (!typeof(FsmState<T>).IsAssignableFrom(stateType))
            {
                throw new KuusouEngineException($"State type '{stateType.FullName}' is invalid.");
            }

            FsmState<T> state = GetState(stateType);
            if (state is null)
            {
                throw new KuusouEngineException($"The fsm '{new TypeNamePair(typeof(T), Name)}' does not exist state '{stateType.Name}'");
            }

            _currentStateTime = 0f;
            _currentState = state;
            _currentState.OnEnter(this);
        }

        public override void Update(float elapseFrequency, float elapseFrequencyReally)
        {
            if (_currentState is null)
            {
                return;
            }
            _currentStateTime += elapseFrequency;
            _currentState.OnUpdate(this, elapseFrequency, elapseFrequencyReally);
        }

        public override void Shutdown()
        {
            ReferencePool.Release(this);
        }

        internal void SwitchToState<TState>() where TState : FsmState<T>
        {
            SwitchToState(typeof(TState));
        }

        internal void SwitchToState(Type stateType)
        {
            if (_currentState is null)
            {
                throw new KuusouEngineException("Current state is invalid.");
            }

            FsmState<T> state = GetState(stateType);
            if (state is null)
            {
                throw new KuusouEngineException($"The fsm '{new TypeNamePair(typeof(T), Name)}' does not exist state '{stateType.Name}'");
            }

            _currentState.OnLeave(this, false);
            _currentStateTime = 0f;
            _currentState = state;
            _currentState.OnEnter(this);
        }
    }
}
