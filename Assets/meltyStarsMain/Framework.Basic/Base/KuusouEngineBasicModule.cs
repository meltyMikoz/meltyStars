namespace KuusouEngine
{
    /// <summary>
    /// 框架基础模块抽象类
    /// </summary>
    internal abstract class KuusouEngineBasicModule
    {
        /// <summary>
        /// 模块优先级
        /// </summary>
        /// <value></value>
        internal int Priority
        {
            get;
        }
        /// <summary>
        /// 模块轮询
        /// </summary>
        /// <param name="elapseFrequency">时间流逝频率(以秒为单位)</param>
        /// <param name="elapseFrequencySteady">真实时间流逝频率(以秒为单位)</param>
        internal abstract void Update(float elapseFrequency, float elapseFrequencyReally);
        /// <summary>
        /// 模块关闭
        /// </summary>
        internal abstract void ShutDown();
    }
}