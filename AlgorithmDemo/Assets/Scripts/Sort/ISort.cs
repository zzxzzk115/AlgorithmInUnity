using System;

namespace LazyRuntime
{
    /// <summary>
    /// 排序接口。
    /// </summary>
    public interface ISort
    {
        /// <summary>
        /// 对数组排序。
        /// </summary>
        /// <typeparam name="T">数组元素类型。</typeparam>
        /// <param name="array">数组。</param>
        void Sort<T>(T[] array) where T : IComparable;
    }
}

