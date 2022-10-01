using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MeltyStars
{
    public class GameActorMoveComponent : AComponent
    {
        public Transform transform;
        protected override void OnAwakeCore()
        {
            this.transform = (Parent as GameActorEntity).gameObject.transform;
        }
        protected override void OnDestroyCore()
        {
            transform = null;
        }
    }
}
