using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cysharp.Threading.Tasks;
using MeltyStars.UI;
using SyntheticWatermelonLikeGame;
using DG.Tweening;

namespace MeltyStars
{
    public partial class SceneStateManagerComponent : AComponent
    {
        private ASceneState m_CurrentState;
        public ASceneState CurrentState => m_CurrentState;
        private Dictionary<Type, ASceneState> m_SceneStateCache;
    }
    public partial class SceneStateManagerComponent
    {
        protected override void OnAwakeCore()
        {
            m_SceneStateCache = new Dictionary<Type, ASceneState>();
        }
        protected override void OnDestroyCore()
        {

        }
        protected override void OnUpdateCore()
        {
            m_CurrentState?.OnUpdate();
        }
        protected override void OnLateUpdateCore()
        {
            m_CurrentState?.OnLateUpdate();
        }
        protected override void OnFixedUpdateCore()
        {
            m_CurrentState?.OnFixedUpdate();
        }
    }
    public partial class SceneStateManagerComponent
    {
        public UniTask SetState<TState>() where TState : ASceneState
        {
            return SetStateCore(typeof(TState));
        }
        public UniTask SetState(Type stateType)
        {
            if (!stateType.IsSubclassOf(typeof(ASceneState)))
            {
                Debug.LogError($"{stateType} is not a sub class of ASceneState!");
                return default(UniTask);
            }
            return SetStateCore(stateType);
        }
        private async UniTask SetStateCore(Type stateType)
        {
            if (!m_SceneStateCache.ContainsKey(stateType))
                RegisterSceneState(stateType);
            ASceneState nextState = m_SceneStateCache[stateType];
            if (m_CurrentState != null)
                await m_CurrentState.OnExitState(nextState);
            GC.Collect();
            await LoadSceneAsync(nextState.Name);
            await nextState.OnEnterState(m_CurrentState);
            m_CurrentState = nextState;
        }
        private void RegisterSceneState(Type stateType)
        {
            ASceneState sceneState = Activator.CreateInstance(stateType) as ASceneState;
            sceneState.OnEnter += prev => OnEnterSceneState();
            sceneState.OnExit += next => OnExitSceneState();
            m_SceneStateCache.Add(stateType, sceneState);
        }
        private async UniTask OnEnterSceneState()
        {
            await UIManagerComponent.Instance.HideWindow<SceneLoadWindow>();
        }
        private async UniTask OnExitSceneState()
        {
            await UIManagerComponent.Instance.ShowWindow<SceneLoadWindow>(UIManagerComponent.EUIRoot.PopUp);
        }
    }
}
