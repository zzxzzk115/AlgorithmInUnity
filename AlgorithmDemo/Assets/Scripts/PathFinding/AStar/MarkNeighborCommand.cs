namespace LazyRuntime
{
    /// <summary>
    /// 标记邻居命令
    /// </summary>
    public class MarkNeighborCommand<T> : BaseCommand<T>
        where T : AStarNode
    {
        private AStarUnityTile m_tool;

        public MarkNeighborCommand(T actor, AStarUnityTile tool) : base(actor)
        {
            m_tool = tool;
        }

        public override void Do()
        {
            m_tool.MarkNode(m_actor, 5);
        }

        public override void Redo()
        {
            // TODO
        }

        public override void Undo()
        {
            // TODO
        }
    }

}

