using System;
using System.Linq;

namespace KuusouEngine
{
    public abstract partial class AEntity
    {
        /// <summary>
        /// 移除第一个对应类型的Child
        /// </summary>
        /// <typeparam name="TChildType"></typeparam>
        public void RemoveFirstChildOf<TChildType>() where TChildType : AEntity
        {
            Type childType = typeof(TChildType);
            if (!Children.ContainsKey(childType))
            {
                StarLogger.LogError($"[{this.GetType()}] InstanceID : {InstanceId} 没有类型为{childType}的子物体表，移除失败");
                return;
            }
            TChildType childEntity = Children[childType].ElementAt(0).Value as TChildType;
            //触发OnRemoveChild
            this.RemoveChild<TChildType>(childEntity);
        }
        /// <summary>
        /// 根据实例移除Child
        /// </summary>
        /// <param name="childEntity"></param>
        /// <typeparam name="TChildType"></typeparam>
        public void RemoveChild<TChildType>(AEntity childEntity) where TChildType : AEntity
        {
            Type childType = typeof(TChildType);
            RemoveChildCore(childType, childEntity);
        }
        public void RemoveChild(AEntity childEntity)
        {
            RemoveChild(childEntity.GetType(), childEntity);
        }
        public void RemoveChild(Type childType, AEntity childEntity)
        {
            RemoveChildCore(childType, childEntity);
        }
        private void RemoveChildCore(Type childType, AEntity childEntity)
        {
            if (!Children.ContainsKey(childType))
            {
                StarLogger.LogError($"{this.GetType()}] InstanceID : {InstanceId} 没有类型为{childType}的子物体表，移除失败");
                return;
            }
            if (!Children[childType].ContainsKey(childEntity.InstanceId))
            {
                StarLogger.LogInfo($"[{this.GetType()}] InstanceID : {InstanceId} 没有类型为{childType} InstanceID : {childEntity.InstanceId} 的子物体");
                return;
            }
            childEntity._parent = null;
            this.Children[childType].Remove(childEntity.InstanceId);
            //触发OnRemoveChild
            this.OnRemoveChild(childEntity);
            if (Children[childType].Count == 0)
                Children.Remove(childType);
        }
        public void RemoveChildById(string instanceID)
        {
            AEntity child = default(AEntity);
            if (child = GetChildById(instanceID))
                this.RemoveChild(child.GetType(), child);
        }
        public void RemoveChildById<TChildType>(string instanceID) where TChildType : AEntity
        {
            TChildType child = default(TChildType);
            if (child = GetChildById<TChildType>(instanceID))
                this.RemoveChild<TChildType>(child);
        }
        public void RemoveChildById(Type childType, string instanceID)
        {
            AEntity child = default(AEntity);
            if (child = GetChildById(childType, instanceID))
                this.RemoveChild(childType, child);
        }
    }
}
