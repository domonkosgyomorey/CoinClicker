using System.Collections;

namespace CoinClicker
{
    public class CircularBuffer<T> : IEnumerable<T>
    {
        private T[] buffer;
        private int head;
        private int tail;
        private int size;
        private int capacity;

        public int Count { get => size; }
        public int Capacity { get => size; }

        public CircularBuffer(int capacity)
        {
            this.capacity = capacity;
            buffer = new T[capacity];
            head = 0;
            tail = 0;
            size = 0;
        }

        public void Add(T item)
        {
            if (size == capacity)
            {
                head = (head + 1) % capacity;
            }
            else
            {
                size++;
            }

            buffer[tail] = item;
            tail = (tail + 1) % capacity;
        }

        public T this[int index]
        {
            get
            {
                if (index < 0 || index >= size)
                    throw new IndexOutOfRangeException();

                return buffer[(head + index) % capacity];
            }
            set
            {
                if (index < 0 || index >= size)
                    throw new IndexOutOfRangeException();

                buffer[(head + index) % capacity] = value;
            }
        }



        public IEnumerator<T> GetEnumerator()
        {
            for (int i = 0; i < size; i++)
            {
                yield return buffer[(head + i) % capacity];
            }
        }

        public List<T> ToList()
        {
            return [.. buffer];
        }

        public static CircularBuffer<T> FromList(List<T> values, int cap)
        {
            if (cap < 3)
            {
                cap = 3;
            }
            CircularBuffer<T> buff = new CircularBuffer<T>(cap);
            foreach (var value in values) buff.Add(value);
            return buff;
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

    }
}
