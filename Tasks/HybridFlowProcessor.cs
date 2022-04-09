using System;
using System.Linq;
using Tasks.DoNotChange;

namespace Tasks
{
    public class HybridFlowProcessor<T> : IHybridFlowProcessor<T>
    {
        private DoublyLinkedList<T> _storage;
        public HybridFlowProcessor()
        {
            _storage = new DoublyLinkedList<T>();
        }

        public T Dequeue() => _storage.Length == 0
            ? throw new InvalidOperationException("Doubly linked list is empty.")
            : _storage.RemoveAt(0);

        public void Enqueue(T item) => _storage.Add(item);

        public T Pop() => _storage.Length == 0
            ? throw new InvalidOperationException("Doubly linked list is empty.")
            : _storage.RemoveAt(_storage.Length - 1);

        public void Push(T item) => _storage.Add(item);
    }
}
