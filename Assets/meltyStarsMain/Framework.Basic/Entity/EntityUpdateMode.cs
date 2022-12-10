namespace KuusouEngine.EngineBasic.Entity
{
    /// <summary>
    /// 实体轮询模式
    /// </summary>
    public enum EntityUpdateMode
    {
        /// <summary>
        /// 普通(跟随父级，父级关闭则停止轮询)
        /// </summary>
        Normal,
        /// <summary>
        /// 总是(忽略父级)
        /// </summary>
        Always
    }
}
