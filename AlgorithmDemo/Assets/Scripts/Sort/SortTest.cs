using System;
using LazyRuntime;
using UnityEngine;

/// <summary>
/// 测试类。
/// </summary>
public class SortTest : MonoBehaviour
{
    public enum SorterType
    {
        Bucket, // 桶排序
        Bubble, // 冒泡排序
        Quick,  // 快速排序
    }

    // 排序类型枚举
    public SorterType m_SorterType;

    // 是否升序排列
    public bool isAscending = true;

    // 排序器
    private Sorter m_Sorter;

    void Start()
    {
        m_Sorter = GetSorter(m_SorterType);
        m_Sorter.IsAscending = isAscending;
        Debug.Log(m_Sorter);
        int[] array = new[] {21, 111, 52, 3333, 999};
        SortAndShow(array, m_Sorter);
        float[] array1 = new[] { 555.1f, 33.1f, 2.2f, 0.1f, 11111.1f};
        SortAndShow(array1, m_Sorter);
        Person[] array2 = new[] {new Person(21, "xsf"), new Person(22, "zkx"),
            new Person(3, "???"), new Person(90, "test"), new Person(33, "noo") };
        SortAndShow(array2, m_Sorter);
    }

    private Sorter GetSorter(SorterType type)
    {
        Sorter sorter = null;
        switch (type)
        {
            case SorterType.Bubble:
                sorter = new BubbleSorter();
                break;
            case SorterType.Bucket:
                sorter = new BucketSorter();
                break;
            case SorterType.Quick:
                sorter = new QuickSorter();
                break;
        }
        return sorter;
    }

    private void SortAndShow<T>(T[] array, ISort sorter) where T:IComparable
    {
        Debug.Log("排序前:");
        LogArrayData(array);
        sorter.Sort(array);
        Debug.Log("排序后:");
        LogArrayData(array);
    }

    private void LogArrayData<T>(T[] array)
    {
        string temp = "";
        foreach (var value in array)
        {
            temp += value + "    ";
        }
        Debug.Log(temp);
    }
}
