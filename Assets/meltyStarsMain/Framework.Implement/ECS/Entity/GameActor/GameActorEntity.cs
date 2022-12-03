using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MeltyStars;
using System;

namespace MeltyStars
{
    public class GameActorEntity : AEntity, IAwake, IDestroy
    {
        public GameObject gameObject { get; protected set; }
        public GameActorEntity(string name)
        {
            gameObject = ObjectScheduler.Instance.GetGameObject(name);
        }
        public GameActorEntity(GameObject gameObject)
        {
            this.gameObject = gameObject;
        }
        public void OnAwakeCore()
        {
            AddComponent<GameActorMoveComponent>();
        }
        public void OnDestroyCore()
        {
            try
            {
                ObjectScheduler.Instance.PushGameObject(gameObject);
            }
            catch (Exception e) when (e is MissingReferenceException)
            {
                Debug.Log(e);
            }
            RemoveComponent<GameActorMoveComponent>();
            gameObject = null;
        }
    }
}
