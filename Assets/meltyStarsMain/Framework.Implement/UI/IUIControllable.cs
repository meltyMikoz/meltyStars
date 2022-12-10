using Cysharp.Threading.Tasks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KuusouEngine.UI
{
    public interface IUIControllable
    {
        UniTask OnShow(object args);
        UniTask OnHide();
        void OnUpdate();
    }
}
