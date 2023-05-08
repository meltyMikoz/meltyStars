using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KuusouEngine.EngineBasic.Event
{
    internal sealed partial class EventManager
    {
        /// <summary>
        /// 事件信息
        /// </summary>
        private struct EventInfo
        {
            public Type EventType { get; set; }
            public object Sender { get; set; }
            public IEvent EventArgs { get; set; }
            public IEventProxyOwner EventProxyOwner { get; set; }
        }
    }
}
