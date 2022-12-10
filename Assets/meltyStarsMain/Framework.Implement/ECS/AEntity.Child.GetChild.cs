using System;
using System.Linq;

namespace KuusouEngine
{
    public abstract partial class AEntity
    {
        public TChildType GetFirstChildOf<TChildType>() where TChildType : AEntity
        {
            Type childType = typeof(TChildType);
            if (typeof(AComponent).IsAssignableFrom(childType))
            {
                StarLogger.LogError($"不能获取Component类型 -> {childType}");
                return null;
            }
            if (!Children.ContainsKey(childType))
            {
                StarLogger.LogError($"[{this.GetType()}] InstanceID : {InstanceId} 没有类型为{childType}的子物体表，获取失败");
                return null;
            }
            return Children[childType].ElementAt(0).Value as TChildType;
        }
        public AEntity GetChildById(string instanceID)
        {
            AEntity entity = default(AEntity);
            foreach (var kv1 in Children)
            {
                foreach (var kv2 in kv1.Value)
                {
                    if (kv2.Key == instanceID)
                        entity = kv2.Value;
                }
            }
            return entity;
        }
        public TChildType GetChildById<TChildType>(string instanceID) where TChildType : AEntity
        {
            return GetChildByIdCore(typeof(TChildType), instanceID) as TChildType;
        }
        public AEntity GetChildById(Type childType, string instanceID)
        {
            return GetChildByIdCore(childType, instanceID);
        }
        private AEntity GetChildByIdCore(Type childType, string instanceID)
        {
            AEntity entity = default(AEntity);
            if (!Children.ContainsKey(childType))
            {
                StarLogger.LogError($"[{this.GetType()}] InstanceID : {InstanceId} 没有类型为{childType}的子物体表，不能获取");
                return entity;
            }
            if (Children[childType].ContainsKey(instanceID))
                entity = Children[childType][instanceID];
            if (!entity)
                StarLogger.LogError($"[{this.GetType()}] InstanceID : {InstanceId} 没有类型为{childType} InstanceID : {instanceID} 的子物体");
            return entity;
        }
    }
}
