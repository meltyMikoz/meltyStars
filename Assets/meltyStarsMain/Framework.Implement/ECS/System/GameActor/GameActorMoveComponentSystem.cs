using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KuusouEngine
{
    public static class GameActorMoveComponentSystem
    {
        public static void MoveToGlobalPositionImmediately(this GameActorMoveComponent self, Vector3 position)
        {
            self.transform.position = position;
        }
        public static void MoveToLocalPositionImmediately(this GameActorMoveComponent self, Vector3 position)
        {
            self.transform.localPosition = position;
        }
    }
}
