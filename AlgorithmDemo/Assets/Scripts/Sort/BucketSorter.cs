using System;
using System.Collections.Generic;

namespace LazyRuntime
{
    /// <summary>
    /// 桶排序器。
    /// </summary>
    public class BucketSorter : Sorter, ISort
    {
        /// <summary>
        /// 计算Hash值（下标），用于分桶。
        /// </summary>
        /// <typeparam name="T">数组元素类型。</typeparam>
        /// <param name="value">待分桶的值。</param>
        /// <param name="length">数组总长度。</param>
        /// <param name="max">数组中的最大值。</param>
        /// <returns>计算得到的Hash值（下标）。</returns>
        private int Hash<T>(T value, int length, T max)
        {
            // 利用.NET 4.x的dynamic特性，可以做四则运算。前提是T类型支持或重载了四则运算符，否则会抛出异常。
            dynamic d_value = value;
            dynamic d_max = max;
            int hashValue = 0;
            // 异常捕获
            try
            {
                hashValue = ((int)(d_value * length) / (int)(d_max + 1));
            }
            catch (Exception e)
            {
                throw e;
            }
            return hashValue;
        }

        /// <summary>
        /// 按大小找到待插入桶中位置，插入值到桶中。
        /// </summary>
        /// <typeparam name="T">节点元素类型。</typeparam>
        /// <param name="bucket">待插入的桶。</param>
        /// <param name="value">待插入的值。</param>
        private void InsertInto<T>(LinkedList<T> bucket, T value) where T : IComparable
        {
            LinkedListNode<T> nextNode = bucket.First;
            LinkedListNode<T> currentNode = null;
            // 如果桶中没有节点，则直接添加新的值为value的节点
            if (nextNode == null)
            {
                bucket.AddFirst(new LinkedListNode<T>(value));
            }
            else
            {
                // 找到待插入的结点位置
                while (nextNode != null && nextNode.Value.CompareTo(value) < 0)
                {
                    currentNode = nextNode;
                    nextNode = nextNode.Next;
                }
                // 插入节点到桶中
                if (currentNode == null)
                    bucket.AddFirst(value);
                else
                    bucket.AddAfter(currentNode, new LinkedListNode<T>(value));
            }
        }

        /// <summary>
        /// 排序算法。
        /// </summary>
        /// <typeparam name="T">数组元素类型。</typeparam>
        /// <param name="array">待排序数组。</param>
        public new void Sort<T>(T[] array) where T : IComparable
        {
            // 定义桶的大小
            int length = array.Length;
            // 字典：int->桶
            Dictionary<int, LinkedList<T>> buckets = new Dictionary<int, LinkedList<T>>(length);
            // 找最大值
            T max = array[0];
            for (int i = 0; i < length; i++)
            {
                if (array[i].CompareTo(max) > 0)
                    max = array[i];
            }
            // 入桶
            for (int i = 0; i < length; i++)
            {
                T value = array[i];
                int hashValue = Hash<T>(value, length, max);
                if (!buckets.ContainsKey(hashValue))
                {
                    buckets[hashValue] = new LinkedList<T>();
                }
                InsertInto(buckets[hashValue], value);
            }
            // 出桶
            int j = 0;
            if (IsAscending)
            {
                for (int i = 0; i < length; i++)
                {
                    if (buckets.ContainsKey(i))
                    {
                        foreach (var value in buckets[i])
                        {
                            array[j++] = value;
                        }
                    }
                }
            }
            else
            {
                for (int i = length-1; i >= 0; i--)
                {
                    if (buckets.ContainsKey(i))
                    {
                        LinkedListNode<T> lastNode = buckets[i].Last;
                        while (lastNode != null)
                        {
                            array[j++] = lastNode.Value;
                            lastNode = lastNode.Previous;
                        }
                    }
                }
            }
            
        }

    }
}

