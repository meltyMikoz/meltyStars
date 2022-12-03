using System;
using System.Collections.Generic;

namespace MeltyStars
{
    public abstract class AComponent : AEntity
    {
        protected internal void OnAddedAsComponent(AEntity entity)
        {
            this.Awake();
            (this as IAddedAsComponent)?.OnAddedAsComponentCore(entity);
        }
        protected internal void OnRemovedFromComponent(AEntity entity)
        {
            (this as IRemovedFromComponent)?.OnRemovedFromComponentCore(entity);
        }
    }
}
