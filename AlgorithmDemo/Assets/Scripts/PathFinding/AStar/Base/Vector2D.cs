namespace LazyRuntime
{
    /// <summary>
    /// 2D向量类型。
    /// </summary>
    public class Vector2D
    {
        public Vector2D() { }
        public Vector2D(int x, int y)
        {
            m_X = x;
            m_Y = y;
        }

        protected int m_X;
        public int X
        {
            get => m_X;
            set => m_X = value;
        }

        protected int m_Y;
        public int Y
        {
            get => m_Y;
            set => m_Y = value;
        }

        public static Vector2D operator +(Vector2D v1, Vector2D v2)
        {
            return new Vector2D(v1.m_X + v2.m_X, v1.m_Y + v2.m_Y);
        }

        public override string ToString()
        {
            return "(" + m_X + "," + m_Y + ")";
        }
    }
}

