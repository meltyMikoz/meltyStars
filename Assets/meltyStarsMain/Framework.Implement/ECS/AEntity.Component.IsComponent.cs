using System;

namespace MeltyStars
{
    public abstract partial class AEntity
    {
        /// <summary>
        /// 是否为组件
        /// </summary>
        /// <returns></returns>
        public bool IsComponent()
        {
            return typeof(AComponent).IsAssignableFrom(this.GetType());
        }
    }
}
