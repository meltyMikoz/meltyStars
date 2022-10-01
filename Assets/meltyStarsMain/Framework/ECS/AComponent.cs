using System;
using System.Collections.Generic;

namespace MeltyStars
{
    public abstract class AComponent : AEntity
    {
        protected internal void OnAddedAsComponent(AEntity entity)
        {
            Awake();
            OnAddedAsComponentCore(entity);
        }
        protected virtual void OnAddedAsComponentCore(AEntity entity) { }
        protected internal void OnRemovedFromComponent(AEntity entity)
        {
            OnRemovedFromComponentCore(entity);
        }
        protected virtual void OnRemovedFromComponentCore(AEntity entity) { }

    }
}
