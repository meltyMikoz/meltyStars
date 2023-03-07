using System;
using System.Collections.Generic;

namespace KuusouEngine
{
    public static class KuusouEngineEntry
    {
        private static readonly LinkedList<KuusouEngineBasicModule> s_kuusouEngineModules;
        static KuusouEngineEntry()
        {
            s_kuusouEngineModules = new LinkedList<KuusouEngineBasicModule>();
        }
        public static void Update(float elapseFrequency, float elapseFrequencyReally)
        {
            foreach (KuusouEngineBasicModule module in s_kuusouEngineModules)
            {
                module.Update(elapseFrequency, elapseFrequencyReally);
            }
        }
        public static void ShutDown()
        {
            for (LinkedListNode<KuusouEngineBasicModule> current = s_kuusouEngineModules.Last; current != null; current = current.Previous)
            {
                current.Value.ShutDown();
            }

            s_kuusouEngineModules.Clear();
            ReferencePool.ClearAll();
            //Utility.Marshal.FreeCachedHGlobal();
            //GameFrameworkLog.SetLogHelper(null);
        }
        public static T GetModule<T>() where T : class
        {
            Type interfaceType = typeof(T);
            if (!interfaceType.IsInterface)
            {
                throw new KuusouEngineException($"You must get module by interface, but '{interfaceType}' is not.");
            }

            if (!interfaceType.FullName.StartsWith("KuusouEngine.", StringComparison.Ordinal))
            {
                throw new KuusouEngineException($"You must get a KuusouEngine module, but '{interfaceType}' is not.");
            }

            string moduleName = $"{interfaceType.Namespace}.{interfaceType.Name.Substring(1)}";
            Type moduleType = Type.GetType(moduleName);
            if (moduleType == null)
            {
                throw new KuusouEngineException($"Can not find Game Framework module type '{moduleName}'");
            }

            return GetModule(moduleType) as T;
        }

        private static KuusouEngineBasicModule GetModule(Type moduleType)
        {
            foreach (KuusouEngineBasicModule module in s_kuusouEngineModules)
            {
                if (module.GetType() == moduleType)
                {
                    return module;
                }
            }

            return CreateModule(moduleType);
        }

        private static KuusouEngineBasicModule CreateModule(Type moduleType)
        {
            KuusouEngineBasicModule module = Activator.CreateInstance(moduleType) as KuusouEngineBasicModule;
            if (module == null)
            {
                throw new KuusouEngineException($"Can not create module '{moduleType.FullName}'.");
            }

            LinkedListNode<KuusouEngineBasicModule> current = s_kuusouEngineModules.First;
            while (current != null)
            {
                if (module.Priority > current.Value.Priority)
                {
                    break;
                }

                current = current.Next;
            }

            if (current != null)
            {
                s_kuusouEngineModules.AddBefore(current, module);
            }
            else
            {
                s_kuusouEngineModules.AddLast(module);
            }

            return module;
        }
    }
}
