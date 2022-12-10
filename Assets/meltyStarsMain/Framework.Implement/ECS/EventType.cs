using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KuusouEngine
{
    public class EventType
    {
        public struct OnAddComponent
        {
            public AEntity Entity;
            public AComponent Component;
        }
        public struct OnRemoveComponent
        {
            public AEntity Entity;
            public AComponent Component;
        }
        public struct OnAddChild
        {
            public AEntity Parent;
            public AEntity Child;
        }
        public struct OnRemoveChild
        {
            public AEntity Parent;
            public AEntity Child;
        }
    }
}
