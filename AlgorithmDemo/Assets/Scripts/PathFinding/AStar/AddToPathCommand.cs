namespace LazyRuntime
{
    /// <summary>
    /// 添加到Path命令
    /// </summary>
    public class AddToPathCommand<T> : BaseCommand<T>
        where T : AStarNode
    {
        private AStarUnityTile m_tool;

        public AddToPathCommand(T actor, AStarUnityTile tool) : base(actor)
        {
            m_tool = tool;
        }

        public override void Do()
        {
            m_tool.MarkNode(m_actor, 2);
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

