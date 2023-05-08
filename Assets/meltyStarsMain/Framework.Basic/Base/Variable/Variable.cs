using System;

namespace KuusouEngine.EngineBasic
{
    public abstract class Variable : IReference
    {
        public abstract Type Type 
        {
            get;
        }
        public abstract object GetValue();
        public abstract void SetValue(object value);
        public abstract void Clear();
    }
    public abstract class Variable<T> : Variable
    {
        private T _value;
        public Variable() 
        {
            _value = default(T);
        }
        public override Type Type
        {
            get
            {
                return typeof(T);
            }
        }
        public T Value
        {
            get
            {
                return _value;
            }
        }
        public override object GetValue()
        {
            return _value;
        }
        public override void SetValue(object value)
        {
            _value = (T)value;
        }
        public override void Clear()
        {
            _value = default(T);
        }
        public override string ToString()
        {
            return (_value is null) ? "<Null variable>" : _value.ToString();
        }
    }
}
