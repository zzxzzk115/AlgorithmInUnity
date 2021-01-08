namespace LazyRuntime
{
    /// <summary>
    /// 点类型。
    /// </summary>
    public struct Point
    {
        public int m_X;

        public int m_Y;

        public Point(int mX, int mY)
        {
            this.m_X = mX;
            this.m_Y = mY;
        }

        public static Point operator +(Point p1, Point p2)
        {
            return new Point(p1.m_X + p2.m_X, p1.m_Y + p2.m_Y);
        }
    }
}

