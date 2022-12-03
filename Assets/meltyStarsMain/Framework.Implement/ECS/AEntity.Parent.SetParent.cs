using System;

namespace MeltyStars
{
    public abstract partial class AEntity
    {
        /// <summary>
        /// 设置父级
        /// </summary>
        /// <param name="parent"></param>
        public void SetParent(AEntity parent)
        {
            this.OnParentChanged(_parent, parent);
        }
    }
}
