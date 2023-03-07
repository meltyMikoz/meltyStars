namespace KuusouEngine.EngineBasic.Entity
{
    internal sealed partial class EntityManager
    {
        /// <summary>
        /// 实体状况
        /// </summary>
        private enum EntityStatus
        {
            /// <summary>
            /// 未知
            /// </summary>
            Unknown = 0,
            /// <summary>
            /// 实体未初始化
            /// </summary>
            UnInited = 1,
            /// <summary>
            /// 实体已初始化
            /// </summary>
            Inited = UnInited << 1,
            /// <summary>
            /// 实体已激活
            /// </summary>
            Actived = UnInited << 2,
            /// <summary>
            /// 实体已失活
            /// </summary>
            Inactived = UnInited << 3,
            /// <summary>
            /// 实体已销毁
            /// </summary>
            Destroyed = UnInited << 4
        }
    }
}
