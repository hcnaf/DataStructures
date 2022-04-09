using System;
using System.Collections;
using System.Collections.Generic;
using Tasks.DoNotChange;

namespace Tasks
{
    public class DoublyLinkedList<T> : IDoublyLinkedList<T>
    {
        private T[] _array = Array.Empty<T>();
        private int _count;
        private int _version;

        public int Length => _count;

        public void Add(T e)
        {
            if (_array.Length == 0)
                _array = new T[1];

            if (_count == _array.Length)
                Array.Resize(ref _array, _array.Length * 2);

            _array[_count++] = e;
            ++_version;
        }

        public void AddAt(int index, T e)
        {
            if (index > _count)
                 throw new IndexOutOfRangeException(nameof(index));

            if (_count == _array.Length)
                Array.Resize(ref _array, _array.Length * 2);

            if (_array.Length == 0)
                _array = new T[1];

            if (index < _count)
                Array.Copy(_array, index, _array, index + 1, _count - index);

            _array[index] = e;
            _count++;
            _version++;
        }

        public T ElementAt(int index) => _array[index];

        public IEnumerator<T> GetEnumerator() => new Enumerator(this);

        public void Remove(T item)
        {
            var index = Array.IndexOf(_array, item);

            if (index == -1)
                return;

            RemoveAt(index);
        }

        public T RemoveAt(int index)
        {
            if (index >= _count)
                throw new IndexOutOfRangeException(nameof(index));

            var res = _array[index];

            if (index < --_count)
                Array.Copy(_array, index + 1, _array, index, _count - index);

            _array[_count] = default;
            _version++;

            return res;
        }

        IEnumerator IEnumerable.GetEnumerator() => new Enumerator(this);

		public struct Enumerator : IEnumerator<T>, IDisposable, IEnumerator
		{
			private DoublyLinkedList<T> _list;

			private int _index;

			private int _version;

			public T Current { get; private set; }

			object IEnumerator.Current
			{
                get => _index == 0 || _index == _list._count + 1
                    ? throw new InvalidOperationException("Enumerator didn't start or already finished.")
                    : Current;
			}

			internal Enumerator(DoublyLinkedList<T> list)
			{
				this._list = list;
				_index = 0;
				_version = list._version;
				Current = default;
			}

			public void Dispose()
			{
			}

			public bool MoveNext()
			{
				if (_version != _list._version)
					throw new InvalidOperationException("Enumerable failed version.");

				if (_index < _list._count)
				{
					Current = _list._array[_index];
					_index++;
					return true;
				}

				_index = _list._count + 1;
				Current = default;
				return false;
			}

			void IEnumerator.Reset()
            {
                if (_version != _list._version)
                    throw new InvalidOperationException("Enumerable failed version.");

                _index = 0;
				Current = default;
			}
		}
	}
}
