using System;
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
    public AStarUnityTile(List<List<int>> map, Tilemap tileMap, Tile[] tiles,
        Text tipText, AStarCommandExecutor commandExecutor, float speed) : base(map)
    {
        m_TileMap = tileMap;
        m_Tiles = tiles;
        m_TipText = tipText;
        m_commandExecutor = commandExecutor;
        m_speed = speed;
        InitTileMap(map);
        // 锁定相机中心到地图中心
        Vector3 mapCenterPosition = m_TileMap.GetCellCenterWorld(GetMapCenterVector3Int(m_Map));
        Camera.main.transform.position = new Vector3(mapCenterPosition.x, mapCenterPosition.y, Camera.main.transform.position.z);
    }

    private Tilemap m_TileMap;

    private Tile[] m_Tiles;

    private Text m_TipText;

    private AStarCommandExecutor m_commandExecutor;

    private float m_speed;

    private bool m_isDoing;
    public bool IsDoing { get => m_isDoing; }

    private bool m_isHasPath;

    public override LinkedList<MapNode> FindPath(MapNode start, MapNode end)
    {
        LinkedList<MapNode> path = new LinkedList<MapNode>();
        AStarNode startNode = start as AStarNode;
        AStarNode endNode = end as AStarNode;
        if (startNode == null || endNode == null)
            throw new InvalidCastException();
        // 把起点放入OpenList
        m_OpenList.Add(startNode);
        m_commandExecutor.AddCommand(new AddToOpenCommand<AStarNode>(startNode, this));

        // 主循环，每一轮检查一个当前方格节点
        while (m_OpenList.Count > 0)
        {
            // 在OpenList中查找F值最小的节点作为当前方格节点
            AStarNode current = FindMinNode(startNode, endNode);

            // 当前方格节点从OpenList中移除
            m_OpenList.Remove(current);

            // 当前方格节点进入CloseList
            m_CloseList.Add(current);

            m_commandExecutor.AddCommand(new AddToCloseCommand<AStarNode>(current, this));

            // 找到所有邻近节点
            List<AStarNode> neighbors = FindNeighbors(current);
            foreach(var neighborNode in neighbors)
            {
                if(!m_CloseList.Contains(neighborNode) && !m_OpenList.Contains(neighborNode))
                {
                    m_commandExecutor.AddCommand(new MarkNeighborCommand<AStarNode>(neighborNode, this));
                }
            }

            foreach (var neighborNode in neighbors)
            {
                if (!m_OpenList.Contains(neighborNode))
                {
                    MarkAndInvolve(current, neighborNode);
                }
                else
                {
                    var neighborNode1 = m_OpenList[m_OpenList.IndexOf(neighborNode)];

                    if (current.G < neighborNode1.PreNode.G)
                    {
                        neighborNode1.PreNode = current;
                        neighborNode1.G = current.G + 1;
                    }
                }
            }
            // 如果终点在OpenList中，直接返回终点格子
            AStarNode findNode = FindNode(m_OpenList, endNode);
            if (findNode != null)
            {
                m_isHasPath = true;
                while (findNode != null)
                {
                    m_commandExecutor.AddCommand(new AddToPathCommand<AStarNode>(findNode, this));
                    path.AddFirst(findNode);
                    findNode = findNode.PreNode;
                }
                return path;
            }
        }
        return null;
    }

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
        m_commandExecutor.Execute(1000 / m_speed, ()=>m_isDoing = true, ()=> {
            if(!m_isHasPath)
            {
                Log("找不到通路！");
            }
        });  
    }

    protected override void MarkAndInvolve(AStarNode current, AStarNode neighborNode)
    {
        if (!m_CloseList.Contains(neighborNode))
        {
            neighborNode.PreNode = current;
            neighborNode.G = current.G + 1;
            m_OpenList.Add(neighborNode);
            m_commandExecutor.AddCommand(new AddToOpenCommand<AStarNode>(neighborNode, this));
        }
    }

    public void Reset()
    {
        m_commandExecutor.Stop();
    }
}
