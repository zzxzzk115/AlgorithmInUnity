namespace LazyRuntime
{
    /// <summary>
    /// A*节点
    /// </summary>
    public class AStarNode : MapNode
    {
        public AStarNode(int x, int y) : base(x, y) { }

        /// <summary>
        /// 前驱节点
        /// </summary>
        public AStarNode PreNode { get; set; }

        /// <summary>
        /// G值
        /// </summary>
        public int G { set; get; }

        public static AStarNode operator +(AStarNode node, Vector2D vector)
        {
            return new AStarNode(node.m_X + vector.X, node.m_Y + vector.Y);
        }

        public override bool Equals(object obj)
        {
            AStarNode node = obj as AStarNode;
            if (node != null)
                return node.m_X == this.m_X && node.m_Y == this.m_Y;
            else
                return false;
        }

        public override int GetHashCode()
        {
            return m_X ^ m_Y;
        }
    }
}

