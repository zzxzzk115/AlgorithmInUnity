using System.Collections.Generic;
using LazyRuntime;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UI;

/// <summary>
/// 用于测试A*算法的MonoBehaviour脚本。
/// </summary>
public class TileAStar : MonoBehaviour
{
    private AStarUnityTile m_AStarUnityTile;

    private List<List<int>> m_Map = new List<List<int>>()
    {
        new List<int>(){ 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
        new List<int>(){ 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
        new List<int>(){ 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
        new List<int>(){ 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
        new List<int>(){ 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
        new List<int>(){ 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
        new List<int>(){ 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 }
    };

    public Tilemap m_TileMap;

    public Tile[] m_Tiles;

    public Text m_TipText;

    [Range(1, 5)]
    public float m_speed = 1;

    private AStarCommandExecutor m_commandExecutor;

    private int m_SetPointChance = 2;

    private AStarNode m_StartNode;

    private AStarNode m_EndNode;

    private void Awake()
    {
        m_commandExecutor = GetComponent<AStarCommandExecutor>();
    }

    void Start()
    {
        ResetScene();
    }

    /// <summary>
    /// 重置场景。
    /// </summary>
    public void ResetScene()
    {
        // 重置AStarUnityTile
        m_AStarUnityTile?.Reset();
        // 重新随机生成障碍物
        RandomGenerateObstacle();
        // 重新设置设置点的机会为2
        m_SetPointChance = 2;
        // 开始点和目标点都设为空
        m_StartNode = null;
        m_EndNode = null;
        // 提示字也设置为空字符串
        m_TipText.text = "";
        // 重新初始化AStarUnityTile实例，该实例继承自AStarUnity，AStarUnity继承自AStar。核心算法在AStar中。
        m_AStarUnityTile = new AStarUnityTile(m_Map, m_TileMap, m_Tiles, m_TipText, m_commandExecutor, m_speed);
    }

    /// <summary>
    /// 随机产生障碍物。
    /// </summary>
    private void RandomGenerateObstacle()
    {
        for (int i = 0; i < m_Map.Count; i++)
        {
            for (int j = 0; j < m_Map[0].Count; j++)
            {
                // 随机设置0或1，7:3开。
                m_Map[i][j] = Random.Range(0, 10) > 7f ? 1 : 0;
            }
        }
    }

    /// <summary>
    /// 开始A*寻路算法。
    /// </summary>
    public void StartAStar()
    {
        // 如果没有设置点的机会，则说明开始点和目标点都已设置完成，可以开始寻路。
        if (m_SetPointChance <= 0 && !m_AStarUnityTile.IsDoing)
        {
            m_AStarUnityTile.FindPathInTileMap(new Vector2Int(m_StartNode.Y, m_StartNode.X),
                new Vector2Int(m_EndNode.Y, m_EndNode.X));
        }
    }

    /// <summary>
    /// 轮询。
    /// </summary>
    void Update()
    {
        // 如果鼠标左键按下
        if (Input.GetMouseButtonDown(0))
        {
            // 获得鼠标的屏幕坐标
            Vector3 mousePosition = Input.mousePosition;
            // 得到鼠标的世界坐标
            Vector3 mouseWorldPosition = Camera.main.ScreenToWorldPoint(mousePosition);
            // 得到鼠标的Tile坐标
            Vector3Int mapCellPosition = m_TileMap.WorldToCell(mouseWorldPosition);
            // 如果该坐标合法（不越界、不是障碍物）
            if (m_AStarUnityTile.CheckValid(mapCellPosition))
            {
                // 如果设置点的机会还剩2次（代表将要设置开始点）
                if (m_SetPointChance == 2)
                {
                    m_StartNode = new AStarNode(mapCellPosition.y, mapCellPosition.x);
                    m_AStarUnityTile.MarkNode(m_StartNode, 3);
                }
                // 否则如果设置点的机会只剩1次（代表将要设置目标点）
                else if (m_SetPointChance == 1)
                {
                    m_EndNode = new AStarNode(mapCellPosition.y, mapCellPosition.x);
                    m_AStarUnityTile.MarkNode(m_EndNode, 4);
                }
                // 设置后要将机会减一
                m_SetPointChance--;
            }
        }
    }
}
