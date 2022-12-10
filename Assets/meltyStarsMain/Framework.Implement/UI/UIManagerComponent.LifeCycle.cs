using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KuusouEngine.UI
{
    public partial class UIManagerComponent : IAwake
    {
        public void OnAwakeCore()
        {
            Instance = this;
        }
    }
}
