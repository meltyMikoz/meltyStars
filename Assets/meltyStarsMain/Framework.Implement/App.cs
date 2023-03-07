using UnityEngine;
using System.Linq;
using System.Collections.Generic;
using System;
using System.Reflection;
using Cysharp.Threading.Tasks;
using KuusouEngine.UI;

namespace KuusouEngine
{
    public class App : AEntity, IAwake
    {
        public static App Instance => Inner.instance;
        private App() { RegisterTypes(); }
        private class Inner
        {
            private Inner() { }
            internal static readonly App instance = new App();
        }
        private Dictionary<string, Type> m_allTypes;
        public Dictionary<string, Type> GetAllTypes()
        {
            return m_allTypes;
        }
        /// <summary>
        /// Game Initialize
        /// </summary>
        public void OnAwakeCore()
        {
            AddComponent<SceneStateManagerComponent>();
            AddComponent<UIManagerComponent>();
        }
        private void RegisterTypes()
        {
            m_allTypes = typeof(App).Assembly.GetTypes()
                                             .ToDictionary(type => type.FullName);
        }
    }
}
