using System.Collections.Generic;
using LazyRuntime;
using UnityEngine;

/// <summary>
/// AStar基础逻辑测试
/// </summary>
public class AStarTest : MonoBehaviour
{
    private List<List<int>> m_Map = new List<List<int>>()
    {
        new List<int>(){ 0, 0, 0, 0, 0, 0, 0 },
        new List<int>(){ 0, 0, 0, 1, 0, 0, 0 },
        new List<int>(){ 0, 0, 0, 1, 0, 0, 0 },
        new List<int>(){ 0, 0, 0, 1, 0, 0, 0 },
        new List<int>(){ 0, 0, 0, 0, 0, 0, 0 }
    };

    private void PrintMap()
    {
        Debug.Log("地图：");
        string temp = "";
        foreach (var list in m_Map)
        {
            foreach (var item in list)
            {
                temp += item + "    ";
            }
            Debug.Log(temp);
            temp = "";
        }
        
    }

    void Start()
    {
        PrintMap();
        AStarNode start = new AStarNode(2, 1);
        AStarNode end = new AStarNode(2, 5);
        AStarUnity aStarUnity = new AStarUnity(m_Map);
        LinkedList<MapNode> path = aStarUnity.FindPath(start, end);
        Debug.Log("从" + start + "到" + end);
        aStarUnity.PrintLinkedList(path);
    }

}
