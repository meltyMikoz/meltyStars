namespace KuusouEngine
{
    public interface IKuusouEngineBasicModule
    {
    }
    /// <summary>
    /// 框架基础模块抽象类
    /// </summary>
    internal abstract class KuusouEngineBasicModule : IKuusouEngineBasicModule
    {
        /// <summary>
        /// 模块优先级
        /// </summary>
        /// <value></value>
        internal virtual int Priority
        {
            get
            {
                return 0;
            }
        }
        /// <summary>
        /// 模块轮询
        /// </summary>
        /// <param name="elapseFrequency">时间流逝频率(以秒为单位)</param>
        /// <param name="elapseFrequencyReally">真实时间流逝频率(以秒为单位)</param>
        internal abstract void Update(float elapseFrequency, float elapseFrequencyReally);
        /// <summary>
        /// 模块关闭
        /// </summary>
        internal abstract void ShutDown();
    }
}