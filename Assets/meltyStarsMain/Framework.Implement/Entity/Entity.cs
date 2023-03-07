using KuusouEngine.EngineBasic.Entity;

namespace KuusouEngine.EngineImplement.Entity
{
    public abstract class Entity : IEntity
    {
        private int _id;
        private IEntityGroup _entityGroup;
        private IEntityInfo _entityInfo;
        public int Id
        {
            get
            {
                return this._id;
            }
            set
            {
                this._id = value;
            }
        }

        public IEntityGroup EntityGroup
        {
            get
            {
                return this._entityGroup;
            }
            set
            {
                this._entityGroup = value;
            }
        }

        public IEntityInfo EntityInfo
        {
            get
            {
                return this._entityInfo;
            }
            set
            {
                this._entityInfo = value;
            }
        }
    }
}