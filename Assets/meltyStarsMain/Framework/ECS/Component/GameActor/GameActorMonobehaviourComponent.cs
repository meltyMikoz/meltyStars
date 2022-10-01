using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MeltyStars
{
    public class GameActorMonobehaviourComponent<T> : AComponent where T : MonoBehaviour
    {
        public T Mono;
        protected override void OnAwakeCore()
        {
            if ((Parent as GameActorEntity).gameObject.TryGetComponent<T>(out T component))
            {
                Mono = component;
                return;
            }
            try
            {
                Mono = (Parent as GameActorEntity).gameObject.AddComponent<T>();
            }
            catch (Exception e)
            {
                Debug.LogError(e);
            }
        }
        protected override void OnDestroyCore()
        {
            Mono = null;
        }
    }
}
