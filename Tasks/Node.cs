using System;
using System.Collections.Generic;
using System.Text;

namespace Tasks
{
    internal class Node<T>
    {
        internal T data;
        internal Node<T> next;
        internal Node<T> previous;
        internal DoublyLinkedList<T> doublyLinkedList;

        public Node(DoublyLinkedList<T> doublyLinkedList, T value)
        {
            this.doublyLinkedList = doublyLinkedList;
            this.data = value;
        }
    }
}
