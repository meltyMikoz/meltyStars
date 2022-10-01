using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MeltyStars.UI
{
    public partial class UIManagerComponent
    {
        protected override void OnAwakeCore()
        {
            Instance = this;
        }
    }
}
