using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KuusouEngine
{
    public class GameActorMonobehaviourComponent<T> : AComponent, IAwake, IDestroy where T : MonoBehaviour
    {
        public T Mono;
        public void OnAwakeCore()
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
        public void OnDestroyCore()
        {
            Mono = null;
        }
    }
}
