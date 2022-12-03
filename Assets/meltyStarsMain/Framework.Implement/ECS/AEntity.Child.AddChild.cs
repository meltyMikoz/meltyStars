using System;
using System.Collections.Generic;

namespace MeltyStars
{
    public abstract partial class AEntity
    {
        /// <summary>
        /// 添加Child
        /// </summary>
        /// <param name="childEntity"></param>
        /// <typeparam name="TChildType"></typeparam>
        public void AddChild<TChildType>(TChildType childEntity) where TChildType : AEntity
        {
            Type childType = typeof(TChildType);
            if (typeof(AComponent).IsAssignableFrom(childType))
            {
                StarLogger.LogError($"不能添加Component为Child -> {childType}");
                return;
            }
            AddChildCore(childType, childEntity);
        }
        /// <summary>
        /// 尝试创建并添加Child
        /// </summary>
        /// <param name="fromPool"></param>
        /// <typeparam name="TChildType"></typeparam>
        /// <returns></returns>
        public TChildType AddChild<TChildType>(bool fromPool = false) where TChildType : AEntity
        {
            Type childType = typeof(TChildType);
            TChildType childEntity;
            if (typeof(AComponent).IsAssignableFrom(childType))
            {
                StarLogger.LogError($"不能添加Component为Child -> {childType}");
                return null;
            }
            if (fromPool)
                childEntity = ObjectScheduler.Instance.Get<TChildType>();
            else
                childEntity = Activator.CreateInstance<TChildType>();
            AddChildCore(childType, childEntity);
            return childEntity;

        }
        private void AddChildCore(Type childType, AEntity childEntity)
        {
            if (!Children.ContainsKey(childType))
                Children.Add(childType, new Dictionary<string, AEntity>());
            childEntity._parent = this;
            this.Children[childType].Add(childEntity.InstanceId, childEntity);
            //触发OnAddChild
            this.OnAddChild(childEntity);
        }
    }
}
