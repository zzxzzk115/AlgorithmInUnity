using System;

namespace LazyRuntime
{
    /// <summary>
    /// 快速排序器。
    /// </summary>
    public class QuickSorter : Sorter, ISort
    {
        /// <summary>
        /// 排序算法。
        /// </summary>
        /// <typeparam name="T">数组元素类型。</typeparam>
        /// <param name="array">待排序数组。</param>
        public new void Sort<T>(T[] array) where T : IComparable
        {
            QuickSort(array, 0, array.Length - 1);
        }

        /// <summary>
        /// 快速排序。
        /// </summary>
        /// <typeparam name="T">数组元素类型。</typeparam>
        /// <param name="array">待排序数组。</param>
        /// <param name="left">左游标。</param>
        /// <param name="right">右游标。</param>
        private void QuickSort<T>(T[] array, int left, int right) where T : IComparable
        {
            T standard;
            int i, j;
            if (left > right)
                return;
            standard = array[left];
            i = left;
            j = right;
            while (i != j)
            {
                if (IsAscending)
                {
                    while (array[j].CompareTo(standard) >= 0 && i < j)
                        j--;
                    while (array[i].CompareTo(standard) <= 0 && i < j)
                        i++;
                }
                else
                {
                    while (array[j].CompareTo(standard) <= 0 && i < j)
                        j--;
                    while (array[i].CompareTo(standard) >= 0 && i < j)
                        i++;
                }
                if (i < j)
                    Swap(ref array[i], ref array[j]);
            }

            array[left] = array[i];
            array[i] = standard;

            QuickSort<T>(array, left, i - 1);
            QuickSort<T>(array, i + 1, right);
        }
    }
}


