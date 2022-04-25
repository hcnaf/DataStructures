using System;
using System.Collections;
using System.Collections.Generic;
using Tasks.DoNotChange;

namespace Tasks
{
    public class DoublyLinkedList<T> : IDoublyLinkedList<T>
    {
        private Node<T> _head;
        private Node<T> _current;
        private int _count;
        private int _version;
        public int Length => _count;

        public void Add(T e)
        {
            var newNode = new Node<T>(this, e);
            if (_head is null)
            {
                AddHead(newNode);
                return;
            }

            InsertNodeBefore(_head, newNode);
        }

        public void AddAt(int index, T e)
        {
            if (index < 0 || index > _count)
                throw new IndexOutOfRangeException(nameof(index));

            var newNode = new Node<T>(this, e);
            if (_head is null)
            {
                AddHead(newNode);
                return;
            }

            InsertNodeBefore(NodeAt(index), newNode);
            if (index == 0)
                _head = newNode;
        }

        public T ElementAt(int index) => index < 0 || index >= _count
            ? throw new IndexOutOfRangeException(nameof(index))
            : NodeAt(index).data;

        public void Remove(T item)
        {
            var node = FindFirst(item);
            if (node != null)
                RemoveNode(node);
        }

        public T RemoveAt(int index)
        {
            if (index < 0 || index >= _count)
                throw new IndexOutOfRangeException(nameof(index));

            var node = NodeAt(index);
            if (node != null)
                RemoveNode(node);

            return node.data;
        }

        public IEnumerator<T> GetEnumerator() => new Enumerator(this);

        IEnumerator IEnumerable.GetEnumerator() => new Enumerator(this);

        private void InsertNodeBefore(Node<T> nextNode, Node<T> newNode)
        {
            newNode.previous = nextNode.previous;
            newNode.next = nextNode;
            nextNode.previous.next = newNode;
            nextNode.previous = newNode;
            _version++;
            _count++;
        }

        private void RemoveNode(Node<T> node)
        {
            if (node.next == node)
            {
                _head = null;
            }
            else
            {
                node.next.previous = node.previous;
                node.previous.next = node.next;
                if (_head == node)
                    _head = node.next;
            }

            _count--;
            _version++;
        }

        public struct Enumerator : IEnumerator, IEnumerator<T>
        {
            private DoublyLinkedList<T> _list;

            private Node<T> _node;

            private int _version;

            private T _current;

            private int _index;
            public object Current => _index != 0 && _index != _list._count + 1 ? _current : throw new InvalidOperationException("Enumerator didn't started or already ended.");

            internal Enumerator(DoublyLinkedList<T> list)
            {
                this._list = list;
                _version = list._version;
                _node = list._head;
                _current = default(T);
                _index = 0;
            }

            T IEnumerator<T>.Current => _current;

            public void Dispose() { }

            public bool MoveNext()
            {
                if (_version != _list._version)
                {
                    throw new InvalidOperationException("Doubly linked list was modified.");
                }
                if (_node is null)
                {
                    _index = _list._count + 1;
                    return false;
                }
                _index++;
                _current = _node.data;
                _node = _node.next;
                if (_node == _list._head)
                {
                    _node = null;
                }
                return true;
            }

            public void Reset()
            {
                if (_version != _list._version)
                {
                    throw new InvalidOperationException("Doubly linked list was modified.");
                }
                _current = default(T);
                _node = _list._head;
                _index = 0;
            }
        }

        private void AddHead(Node<T> newNode)
        {
            newNode.next = newNode;
            newNode.previous = newNode;
            _head = newNode;
            _version++;
            _count++;
        }

        private Node<T> NodeAt(int index)
        {
            var res = _head;
            for(int i = 0; i < index; i++)
                res = res.next;

            return res;
        }

        private Node<T> FindFirst(T data)
        {
            var node = _head;
            if (node is null)
                return null;

            if (data is null)
            {
                do
                {
                    if (node.next is null)
                        return node;

                    node = node.next;
                } while (node.next != _head);

                return null;
            }

            do
            {
                if(data.Equals(node.data))
                    return node;

                node = node.next;
            } while (node != _head);

            return null;
        }
    }
}
