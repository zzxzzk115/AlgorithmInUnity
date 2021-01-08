using System.Collections.Generic;
using LazyRuntime;
using UnityEngine;

/// <summary>
/// AStar在Unity中的简单实现。提供给其他更具体的情况（Tile、Sprite等）实现。
/// </summary>
public class AStarUnity : AStar
{
    public AStarUnity(List<List<int>> map) : base(map) { }

    /// <summary>
    /// 打印信息。利用Debug.Log完成。
    /// </summary>
    /// <param name="str">待打印的信息。</param>
    public override void Log(string str)
    {
        Debug.Log(str);
    }

}
