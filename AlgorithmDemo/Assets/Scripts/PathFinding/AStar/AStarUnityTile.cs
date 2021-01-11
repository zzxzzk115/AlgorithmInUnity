using System.Collections.Generic;
using LazyRuntime;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UI;

/// <summary>
/// AStarUnity的进一步实现，基于TileMap实现。
/// </summary>
public class AStarUnityTile : AStarUnity
{
    public AStarUnityTile(List<List<int>> map, Tilemap tileMap, Tile[] tiles, Text tipText) : base(map)
    {
        m_TileMap = tileMap;
        m_Tiles = tiles;
        m_TipText = tipText;
        InitTileMap(map);
        // 锁定相机中心到地图中心
        Vector3 mapCenterPosition = m_TileMap.GetCellCenterWorld(GetMapCenterVector3Int(m_Map));
        Camera.main.transform.position = new Vector3(mapCenterPosition.x, mapCenterPosition.y, Camera.main.transform.position.z);
    }

    private Tilemap m_TileMap;

    private Tile[] m_Tiles;

    private Text m_TipText;

    /// <summary>
    /// 得到地图中心的坐标。
    /// </summary>
    /// <param name="map">地图数据。</param>
    /// <returns>计算得到的三维坐标（int、值类型）</returns>
    private Vector3Int GetMapCenterVector3Int(List<List<int>> map)
    {
        int x = map[0].Count / 2;
        int y = map.Count / 2;
        return new Vector3Int(x, y, 0);
    }

    /// <summary>
    /// 初始化TileMap
    /// </summary>
    /// <param name="map">地图数据。</param>
    private void InitTileMap(List<List<int>> map)
    {
        for (int i = 0; i < map.Count; i++)
        {
            for (int j = 0; j < map[0].Count; j++)
            {
                m_TileMap.SetTile(new Vector3Int(j, i, 0), m_Tiles[map[i][j]]);
            }
        }
    }

    /// <summary>
    /// 重写自AStar的Log方法。打印消息。添加了更新文本的操作。
    /// </summary>
    /// <param name="str">待打印、显示在Text组件的消息。</param>
    public override void Log(string str)
    {
        base.Log(str);
        m_TipText.text = str;
    }

    /// <summary>
    /// 设置给定节点对应坐标的Tile。
    /// </summary>
    /// <param name="node">给定节点。</param>
    /// <param name="TileTypeID">要设置的Tile的种类ID。</param>
    public void MarkNode(AStarNode node, int TileTypeID)
    {
        m_TileMap.SetTile(new Vector3Int(node.Y, node.X, 0), m_Tiles[TileTypeID]);
    }

    /// <summary>
    /// 检查给定位置是否合法（无障碍物、没越界）
    /// </summary>
    /// <param name="position"></param>
    /// <returns></returns>
    public bool CheckValid(Vector3Int position)
    {
        AStarNode node = new AStarNode(position.y, position.x);
        if (!CheckBorder(node))
        {
            if (!CheckBarrier(node))
                return true;
            return false;
        }
        return false;
    }

    /// <summary>
    /// 开始A*寻路，找到路径后设置对应的Tile，找不到则提示相关信息。
    /// </summary>
    /// <param name="start">开始点的坐标。</param>
    /// <param name="end">结束点的坐标。</param>
    public void FindPathInTileMap(Vector2Int start, Vector2Int end)
    {
        AStarNode startNode = new AStarNode(start.y, start.x);
        AStarNode endNode = new AStarNode(end.y, end.x);
        LinkedList<MapNode> path = FindPath(startNode, endNode);
        if (path != null)
        {
            foreach (var node in path)
            {
                if(!node.Equals(startNode) && !node.Equals(endNode))
                    MarkNode(node as AStarNode, 2);
            }
        }
        else
            Log("找不到路径！");
        
    }
    
}
