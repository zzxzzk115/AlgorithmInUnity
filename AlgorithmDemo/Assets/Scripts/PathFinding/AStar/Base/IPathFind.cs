using System.Collections.Generic;

namespace LazyRuntime
{
    /// <summary>
    /// 寻路接口。
    /// </summary>
    public interface IPathFind
    {
        /// <summary>
        /// 寻路算法，得到点的集合。
        /// </summary>
        /// <param name="start">出发点。</param>
        /// <param name="end">目标点。</param>
        /// <returns>路径的点的集合。</returns>
        LinkedList<AStarNode> FindPath(AStarNode start, AStarNode end);
    }
}

