using System;
using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace MeltyStars
{
    public abstract partial class ASceneState
    {
        public abstract string Name { get; }
        public ASceneState Prev { get; set; }
        public ASceneState Next { get; set; }
        public event Func<ASceneState, UniTask> OnEnter;
        public event Func<ASceneState, UniTask> OnExit;
    }
    public abstract partial class ASceneState
    {
        public async UniTask OnEnterState(ASceneState prev)
        {
            await OnBeforeEnter(prev);
            if (!object.ReferenceEquals(OnEnter, null))
                await OnEnter.Invoke(prev);
            await OnAfterEnter(prev);
            this.Prev = prev;
        }
        public async UniTask OnExitState(ASceneState next)
        {
            await OnBeforeExit(next);
            if (!object.ReferenceEquals(OnEnter, null))
                await OnExit.Invoke(next);
            await OnAfterExit(next);
            this.Next = next;
        }
    }
    public abstract partial class ASceneState
    {
        protected abstract UniTask OnBeforeEnter(ASceneState prev);
        protected abstract UniTask OnBeforeExit(ASceneState next);
        protected abstract UniTask OnAfterEnter(ASceneState prev);
        protected abstract UniTask OnAfterExit(ASceneState next);
        public abstract void OnUpdate();
        public abstract void OnLateUpdate();
        public abstract void OnFixedUpdate();
    }
}
