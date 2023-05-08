using System;

namespace KuusouEngine.EngineBasic.Entity
{
    public class BindWithSystemAttribute : KuusouEngineAttribute
    {
        public Type SystemType { get; set; }
        public BindWithSystemAttribute(Type systemType) 
        { 
            SystemType = systemType;
        }
        private BindWithSystemAttribute() { }
    }
}
