using System;
using System.Collections.Generic;
using System.Linq;

namespace Learn.Algo.Shared
{
    public class Node
    {
        public int val;
        public Node left;
        public Node right;
        public Node next;

        public Node() { }

        public Node(int _val)
        {
            val = _val;
        }

        public Node(int _val, Node _left, Node _right, Node _next)
        {
            val = _val;
            left = _left;
            right = _right;
            next = _next;
        }
    }
    public class TreeNode
    {
        public int val;
        public TreeNode left;
        public TreeNode right;

        public TreeNode(int val = 0, TreeNode left = null, TreeNode right = null)
        {
            this.val = val;
            this.left = left;
            this.right = right;
        }
    }
    public class ListNode
    {
        public int val;
        public ListNode next;

        public ListNode(int val = 0, ListNode next = null)
        {
            this.val = val;
            this.next = next;
        }

        public int[] ToArray()
        {
            var res = new List<int>();
            var cur = this;
            while (cur != null)
            {
                res.Add(cur.val);
                cur = cur.next;
            }

            return res.ToArray();
        }
    }
    public class Heap<T>
    {
        private List<T> _heap = new List<T>();
        private readonly IComparer<T> _comparer;

        public int HeapSize => _heap.Count;
        public Heap(IComparer<T> comparer)
        {
            _comparer = comparer;
        }
        public T Peek()
        {
            return _heap[0];
        }

        public void Insert(T val)
        {
            _heap.Add(val);
            SiftUp(_heap.Count - 1);
        }

        public T RemoveAndGet()
        {
            Swap(0, _heap.Count - 1);
            var valToRemove = _heap.Last();
            _heap.RemoveAt(_heap.Count - 1);
            SiftDown(0);
            return valToRemove;
        }

        public void SiftUp(int curIdx)
        {
            var parentIdx = (curIdx - 1) / 2;

            while (curIdx > 0 && _comparer.Compare(_heap[curIdx], _heap[parentIdx]) < 0)
            {
                Swap(curIdx, parentIdx);
                curIdx = parentIdx;
                parentIdx = (curIdx - 1) / 2;
            }
        }


        public void SiftDown(int curIdx)
        {
            var childOneIdx = curIdx * 2 + 1;
            while (childOneIdx <= _heap.Count - 1)
            {
                var childTwoIdx = curIdx * 2 + 2;
                if (childTwoIdx > _heap.Count - 1)
                {
                    childTwoIdx = -1;
                }

                int idxToSwap;
                if (childTwoIdx != -1 && _comparer.Compare(_heap[childTwoIdx], _heap[childOneIdx]) < 0)
                {
                    idxToSwap = childTwoIdx;
                }
                else
                {
                    idxToSwap = childOneIdx;
                }

                if (_comparer.Compare(_heap[idxToSwap], _heap[curIdx]) < 0)
                {
                    Swap(idxToSwap, curIdx);
                    curIdx = idxToSwap;
                    childOneIdx = curIdx * 2 + 1;
                }
                else
                {
                    break;
                }
            }
        }


        private void Swap(int i, int j)
        {
            var tmp = _heap[i];
            _heap[i] = _heap[j];
            _heap[j] = tmp;
        }

    }
}
