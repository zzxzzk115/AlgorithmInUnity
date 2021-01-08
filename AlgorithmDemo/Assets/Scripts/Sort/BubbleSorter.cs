using System;

namespace LazyRuntime
{
    /// <summary>
    /// 冒泡排序器。
    /// </summary>
    public class BubbleSorter : Sorter, ISort
    {
        /// <summary>
        /// 排序算法。
        /// </summary>
        /// <typeparam name="T">数组元素类型。</typeparam>
        /// <param name="array">待排序数组。</param>
        public new void Sort<T>(T[] array) where T : IComparable
        {
            int length = array.Length;
            
            for (int i = 0; i < length - 1; i++)
            {
                for (int j = 0; j < length - i - 1; j++)
                {
                    if (IsAscending)
                    {
                        if (array[j].CompareTo(array[j + 1]) > 0)
                        {
                            Swap(ref array[j], ref array[j + 1]);
                        }
                    }
                    else
                    {
                        if (array[j].CompareTo(array[j + 1]) < 0)
                        {
                            Swap(ref array[j], ref array[j + 1]);
                        }
                    }
                    
                }
            }
        }
    }
}

