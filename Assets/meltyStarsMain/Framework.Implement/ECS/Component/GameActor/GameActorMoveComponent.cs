using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KuusouEngine
{
    public class GameActorMoveComponent : AComponent, IAwake, IDestroy
    {
        public Transform transform;
        public void OnAwakeCore()
        {
            this.transform = (Parent as GameActorEntity).gameObject.transform;
        }
        public void OnDestroyCore()
        {
            transform = null;
        }
    }
}
