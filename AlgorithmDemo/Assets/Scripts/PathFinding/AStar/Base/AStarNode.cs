using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LazyRuntime
{
    public class AStarNode : Vector2D
    {
        public AStarNode(int x, int y) : base(x, y) { }

        public AStarNode PreNode { get; set; }

        public int G { set; get; }

        public static AStarNode operator +(AStarNode node, Vector2D vector)
        {
            //AStarNode newNode = new AStarNode(node.m_X + vector.X, node.m_Y + vector.Y);
            //newNode.PreNode = node.PreNode;
            //node.m_X += vector.X;
            //node.m_Y += vector.Y;
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
            return base.GetHashCode();
        }
    }
}

