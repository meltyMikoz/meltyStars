using System;
using System.Collections;
using System.Collections.Generic;
using MeltyStars.Collections.Generic;
using UnityEngine;

namespace MeltyStars.UI
{
    public partial class UIManagerComponent
    {
        /// <summary>
        /// UI×é
        /// </summary>
        public class UIGroup
        {
            public string GroupName { get; set; }
            private StackList<AUIWindow> m_windowStackList;
            private Dictionary<Type, AUIWindow> m_windowCache;
            public UIGroup()
            {
                m_windowStackList = new StackList<AUIWindow>(2);
                m_windowCache = new Dictionary<Type, AUIWindow>(2);
            }
        }
    }
}
