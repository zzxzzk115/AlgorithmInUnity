namespace LazyRuntime
{
    /// <summary>
    /// 基本命令
    /// </summary>
    public abstract class BaseCommand<T>
    {
        /// <summary>
        /// Actor
        /// </summary>
        protected T m_actor;

        public BaseCommand(T actor)
        {
            m_actor = actor;
        }

        /// <summary>
        /// 执行
        /// </summary>
        public abstract void Do();

        /// <summary>
        /// 撤回
        /// </summary>
        public abstract void Undo();

        /// <summary>
        /// 重做
        /// </summary>
        public abstract void Redo();
    }
}


