namespace LazyRuntime
{
    /// <summary>
    /// 添加到OpenList命令
    /// </summary>
    public class AddToOpenCommand<T> : BaseCommand<T>
        where T : AStarNode
    {
        private AStarUnityTile m_tool;

        public AddToOpenCommand(T actor, AStarUnityTile tool) : base(actor)
        {
            m_tool = tool;
        }

        public override void Do()
        {
            m_tool.MarkNode(m_actor, 6);
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

