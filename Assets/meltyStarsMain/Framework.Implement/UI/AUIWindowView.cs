using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MeltyStars.UI
{
    public abstract class AUIWindowView
    {
        public readonly Transform uiTransform;
        public AUIWindowView(Transform uiTransform)
        {
            this.uiTransform = uiTransform;
        }
    }
}
