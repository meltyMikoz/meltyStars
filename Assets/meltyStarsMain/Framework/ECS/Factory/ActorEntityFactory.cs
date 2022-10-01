using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MeltyStars;
using System;

namespace MeltyStars
{
    public class GameActorEntityFactory
    {
        public static GameActorEntity CreateEntity(string name, params Type[] components)
        {
            GameActorEntity gameActorEntity = new GameActorEntity(name);
            return CreateEntityCore(gameActorEntity, components);
        }
        public static GameActorEntity CreateEntity(GameObject gameObject, params Type[] components)
        {
            GameActorEntity gameActorEntity = new GameActorEntity(gameObject);
            return CreateEntityCore(gameActorEntity, components);
        }
        private static GameActorEntity CreateEntityCore(GameActorEntity gameActorEntity, params Type[] components)
        {
            foreach (var component in components)
            {
                gameActorEntity.AddComponent(component);
            }
            gameActorEntity.Awake();
            return gameActorEntity;
        }
        public static GameActorEntity CreateEntityWithParent(string name, AEntity parent, params Type[] components)
        {
            GameActorEntity gameActorEntity = new GameActorEntity(name);
            return CreateEntityWithParentCore(gameActorEntity, parent, components);
        }
        public static GameActorEntity CreateEntityWithParent(GameObject gameObject, AEntity parent, params Type[] components)
        {
            GameActorEntity gameActorEntity = new GameActorEntity(gameObject);
            return CreateEntityWithParentCore(gameActorEntity, parent, components);
        }
        private static GameActorEntity CreateEntityWithParentCore(GameActorEntity gameActorEntity, AEntity parent, params Type[] components)
        {
            parent.AddChild(gameActorEntity);
            foreach (var component in components)
            {
                gameActorEntity.AddComponent(component);
            }
            gameActorEntity.Awake();
            return gameActorEntity;
        }
        public static void DestroyEntity(GameActorEntity actorEntity)
        {
            actorEntity.Destroy(true);
        }
    }
}
