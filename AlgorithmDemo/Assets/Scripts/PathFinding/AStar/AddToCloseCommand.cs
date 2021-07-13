namespace LazyRuntime
{
    /// <summary>
    /// 添加到CloseList命令
    /// </summary>
    public class AddToCloseCommand<T> : BaseCommand<T>
        where T : AStarNode
    {
        private AStarUnityTile m_tool;

        public AddToCloseCommand(T actor, AStarUnityTile tool) : base(actor)
        {
            m_tool = tool;
        }

        public override void Do()
        {
            m_tool.MarkNode(m_actor, 7);
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

