using System;

namespace LazyRuntime
{
    public class Sorter : ISort
    {
        // 是否升序排列
        public bool IsAscending { get; set; }

        /// <summary>
        /// 排序算法。
        /// </summary>
        /// <typeparam name="T">数组元素类型。</typeparam>
        /// <param name="array">待排序数组。</param>
        public void Sort<T>(T[] array) where T : IComparable
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 交换值。
        /// </summary>
        /// <typeparam name="T">数组元素类型。</typeparam>
        /// <param name="t1">待交换值。</param>
        /// <param name="t2">待交换值。</param>
        protected void Swap<T>(ref T t1, ref T t2)
        {
            T temp = t1;
            t1 = t2;
            t2 = temp;
        }

        public override string ToString()
        {
            return "当前排序器：" + base.ToString() + ", 是否升序：" + IsAscending;
        }
    }
}

