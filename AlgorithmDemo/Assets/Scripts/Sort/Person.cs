using System;

/// <summary>
/// 人物类。实现了IComparable接口，以年龄为参考值。
/// </summary>
public class Person : System.IComparable
{
    private int m_Age;
    private string m_Name;

    public Person(int age, string name)
    {
        m_Age = age;
        m_Name = name;
    }

    public int Age
    {
        get => m_Age;
    }

    public string Name
    {
        get => m_Name;
    }

    public int CompareTo(object obj)
    {
        int comp;
        Person pOther = obj as Person;
        if (pOther != null)
            comp = this.m_Age - pOther.m_Age;
        else
        {
            throw new InvalidCastException();
        }
        return comp;
    }

    public override string ToString()
    {
        return "[Person] Age = " + m_Age + ", Name = " + m_Name;
    }

    public static int operator *(Person p, int i)
    {
        return p.m_Age * i;
    }

    public static int operator +(Person p, int i)
    {
        return p.m_Age + i;
    }
}
