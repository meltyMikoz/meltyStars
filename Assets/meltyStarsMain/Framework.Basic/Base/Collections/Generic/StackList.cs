using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KuusouEngine.Collections.Generic
{
    public class StackList<T> : List<T>
    {
        public T Pop()
        {
            if (this.Count == 0)
                throw new KuusouEngineException($"StackList is Empty, Generic type is {typeof(T)}");
            T item = this[Count - 1];
            this.RemoveAt(Count - 1);
            return item;
        }
        public T Peek()
        {
            if (this.Count == 0)
                throw new KuusouEngineException($"StackList is Empty, Generic type is {typeof(T)}");
            return this[Count - 1];
        }
        public void Push(T item)
        {
            this.Add(item);
        }
        public StackList() { }
        public StackList(int capacity) : base(capacity) { }
    }
}
