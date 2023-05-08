using System;
using System.Runtime.InteropServices;

namespace KuusouEngine
{
    [StructLayout(LayoutKind.Auto)]
    internal readonly struct TypeNamePair : IEquatable<TypeNamePair>
    {
        private readonly Type _type;
        private readonly string _name;

        public TypeNamePair(Type type)
            : this(type, string.Empty)
        {
        }
        public TypeNamePair(Type type, string name)
        {
            if (type is null)
            {
                throw new KuusouEngineException("Type is invalid.");
            }
            _type = type;
            _name = name;
        }

        public Type Type 
        { 
            get 
            { 
                return _type; 
            } 
        }

        public string Name
        {
            get 
            {
                return _name;
            }
        }

        public override string ToString()
        {
            string typeName = _type.FullName;
            return string.IsNullOrEmpty(_name) ? typeName : $"{typeName}.{_name}";
        }
        public override int GetHashCode()
        {
            return _type.GetHashCode() ^ _name.GetHashCode();
        }
        public override bool Equals(object obj)
        {
            return obj is TypeNamePair && Equals((TypeNamePair)obj);
        }
        public bool Equals(TypeNamePair other)
        {
            return _type == other._type && _name == other._name;
        }

        public static bool operator ==(TypeNamePair a, TypeNamePair b)
        {
            return a.Equals(b);
        }

        public static bool operator !=(TypeNamePair a, TypeNamePair b)
        {
            return !(a == b);
        }
    }
}
