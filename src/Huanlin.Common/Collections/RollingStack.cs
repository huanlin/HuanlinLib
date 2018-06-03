using System;
using System.Collections.Generic;
using System.Text;

namespace Huanlin.Common.Collections
{
    /// <summary>
    /// 這是可限制最大元素數量的堆疊。
    /// 每當推入新元素時，若元素數量超過最大限制，就會把最早推入的元素丟棄。
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class RollingStack<T> : LinkedList<T>
    {

        private int _maxSize;

        public int MaxSize
        {
            get => _maxSize;
            set
            {
                _maxSize = value;
                if (_maxSize <= 0)
                    throw new InvalidOperationException("MaxSize 屬性必須為大於零的正整數!");

                GuardSize();
            }
        }


        public RollingStack(int maxSize)
        {
            MaxSize = maxSize;
        }

        public void Push(T instance)
        {
            AddFirst(instance);
            GuardSize();
        }

        private void GuardSize()
        {
            while (Count > _maxSize)
            {
                RemoveLast();
            }
        }

        public T Pop()
        {
            if (First != null)
            {
                T instance = First.Value;
                RemoveFirst();
                return instance;
            }
            return default(T);
        }

        public T Peek()
        {
            if (First != null)
                return First.Value;
            return default(T);
        }
    }
}
