namespace MeltyStars
{
    internal abstract class FrameworkBasicModule
    {
        /// <summary>
        /// 模块优先级
        /// </summary>
        /// <value></value>
        internal int Priority
        {
            get;
            private set;
        }
        /// <summary>
        /// 模块轮询
        /// </summary>
        /// <param name="elapseFrequency">时间流逝频率(以秒为单位)</param>
        /// <param name="elapseFrequencySteady">真实时间流逝频率(以秒为单位)</param>
        internal abstract void OnUpdate(float elapseFrequency, float elapseFrequencyReally);
        /// <summary>
        /// 模块关闭
        /// </summary>
        internal abstract void ShutDown();
    }
}