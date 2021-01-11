using System;
using System.Collections.Generic;
using UnityEngine;

namespace LazyRuntime
{
    /// <summary>
    /// A*寻路。
    /// </summary>
    public class AStar : IPathFind
    {

        protected List<AStarNode> m_OpenList = new List<AStarNode>();

        protected List<AStarNode> m_CloseList = new List<AStarNode>();

        protected List<List<int>> m_Map;

        public AStar(List<List<int>> map)
        {
            m_Map = map;
        }

        
        /// <summary>
        /// 寻路算法，得到点的链表。
        /// </summary>
        /// <param name="start">出发点。</param>
        /// <param name="end">目标点。</param>
        /// <returns>路径的点的集合。</returns>
        public LinkedList<MapNode> FindPath(MapNode start, MapNode end)
        {
            LinkedList<MapNode> path = new LinkedList<MapNode>();
            AStarNode startNode = start as AStarNode;
            AStarNode endNode = end as AStarNode;
            if(startNode == null || endNode == null)
                throw new InvalidCastException();
            // 把起点放入OpenList
            m_OpenList.Add(startNode);

            // 主循环，每一轮检查一个当前方格节点
            while (m_OpenList.Count > 0)
            {
                // 在OpenList中查找F值最小的节点作为当前方格节点
                AStarNode current = FindMinNode(startNode, endNode);

                // 当前方格节点从OpenList中移除
                m_OpenList.Remove(current);

                // 当前方格节点进入CloseList
                m_CloseList.Add(current);

                // 找到所有邻近节点
                List<AStarNode> neighbors = FindNeighbors(current);

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
                    while (findNode != null)
                    {
                        path.AddFirst(findNode);
                        findNode = findNode.PreNode;
                    }
                    return path;
                }
            }
            return null;
        }

        /// <summary>
        /// 在一行中打印列表中所有的元素。
        /// </summary>
        /// <typeparam name="T">元素类型。</typeparam>
        /// <param name="list">列表。</param>
        public void PrintList<T>(List<T> list)
        {
            string temp = "";
            foreach (var item in list)
            {
                temp += item.ToString() + " ";
            }
            Log(temp);
        }

        /// <summary>
        /// 在一行中打印链表中所有的元素。
        /// </summary>
        /// <typeparam name="T">元素类型。</typeparam>
        /// <param name="list">链表。</param>
        public void PrintLinkedList<T>(LinkedList<T> list)
        {
            string temp = "";
            foreach (var item in list)
            {
                temp += item.ToString() + " ";
            }
            Log(temp);
        }

        /// <summary>
        /// 打印信息。
        /// </summary>
        /// <param name="str">待打印的字符串。</param>
        public virtual void Log(string str) { }

        /// <summary>
        /// 检查位置上是否是障碍物。
        /// </summary>
        /// <param name="node">检测点。</param>
        /// <returns>是否是障碍物。</returns>
        protected virtual bool CheckBarrier(AStarNode node)
        {
            return m_Map[node.X][node.Y] != 0;
        }

        /// <summary>
        /// 检测节点是否越界。
        /// </summary>
        /// <param name="node">检测点。</param>
        /// <returns>是否越界。</returns>
        protected virtual bool CheckBorder(AStarNode node)
        {
            if (node.X >= m_Map.Count || node.X < 0 || node.Y >= m_Map[0].Count || node.Y < 0)
            {
                return true;
            }
            return false;
        }

        /// <summary>
        /// 在列表中查找与给定节点相等的节点，并返回。
        /// </summary>
        /// <typeparam name="T">元素类型。</typeparam>
        /// <param name="list">列表。</param>
        /// <param name="node">给定节点。</param>
        /// <returns>搜索的节点，若没找到，则返回null。</returns>
        protected virtual AStarNode FindNode<T>(List<T> list, AStarNode node)
        {
            foreach (var item in list)
            {
                AStarNode findNode = item as AStarNode;
                if (findNode != null)
                {
                    if (findNode.Equals(node))
                    {
                        return findNode;
                    }
                }
            }
            return null;
        }

        /// <summary>
        /// 查找给定节点上下左右方向的节点，如果没越界并且不是障碍物，则添加到列表中。
        /// </summary>
        /// <param name="node">给定节点。</param>
        /// <returns>相邻节点的列表。</returns>
        protected virtual List<AStarNode> FindNeighbors(AStarNode node)
        {
            List<AStarNode> neighbors = new List<AStarNode>();
            List<Vector2D> vectors = new List<Vector2D>()
            {new Vector2D(0, -1), new Vector2D(-1, 0), new Vector2D(0, 1), new Vector2D(1, 0)};
            foreach (var v in vectors)
            {
                AStarNode target = node + v;
                
                if (!CheckBorder(target))
                {
                    if (!CheckBarrier(target))
                    {
                        neighbors.Add(target);
                    }
                }
            }
            return neighbors;
        }

        /// <summary>
        /// 在OpenList中查找F值最小的节点，并返回。
        /// </summary>
        /// <param name="start">出发点。</param>
        /// <param name="end">目标点。</param>
        /// <returns>OpenList中F值最小的节点。</returns>
        protected virtual AStarNode FindMinNode(AStarNode start, AStarNode end)
        {
            AStarNode minNode = default(AStarNode);
            int minF = int.MaxValue;
            foreach (var node in m_OpenList)
            {
                int currentF = F(node, end, node.G);
                if (minF >= currentF)
                {
                    minF = currentF;
                    minNode = node;
                }
            }
            return minNode;
        }

        /// <summary>
        /// 计算F值。
        /// </summary>
        /// <param name="p">给定节点。</param>
        /// <param name="end">目标点。</param>
        /// <param name="G">给定G值。</param>
        /// <returns>F值。</returns>
        protected virtual int F(AStarNode p, AStarNode end, int G)
        {
            int H = Math.Abs(end.X - p.X) + Math.Abs(end.Y - p.Y);
            return G + H;
        }

        /// <summary>
        /// 标记相邻节点的前驱、添加相邻节点到OpenList
        /// </summary>
        /// <param name="current">当前节点。</param>
        /// <param name="neighborNode">相邻节点。</param>
        protected virtual void MarkAndInvolve(AStarNode current, AStarNode neighborNode)
        {
            if (!m_CloseList.Contains(neighborNode))
            {
                neighborNode.PreNode = current;
                neighborNode.G = current.G + 1;
                m_OpenList.Add(neighborNode);
            }
        }
    }
}

