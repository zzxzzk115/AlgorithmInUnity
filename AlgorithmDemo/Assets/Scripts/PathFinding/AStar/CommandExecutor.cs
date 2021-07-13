using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LazyRuntime
{
    /// <summary>
    /// 命令执行者
    /// </summary>
    public class CommandExecutor<T> : MonoBehaviour
    {
        /// <summary>
        /// 命令列表
        /// </summary>
        private List<BaseCommand<T>> m_baseCommands = new List<BaseCommand<T>>();

        /// <summary>
        /// 执行协程
        /// </summary>
        private Coroutine m_executeCoroutine;

        /// <summary>
        /// 添加命令
        /// </summary>
        /// <param name="cmd">待添加的命令</param>
        public void AddCommand(BaseCommand<T> cmd)
        {
            m_baseCommands.Add(cmd);
        }

        /// <summary>
        /// 移除命令
        /// </summary>
        /// <param name="cmd">待移除的命令</param>
        public void RemoveCommand(BaseCommand<T> cmd)
        {
            m_baseCommands.Remove(cmd);
        }

        /// <summary>
        /// 开始执行协程
        /// </summary>
        /// <param name="milliseconds">执行间隔时间(毫秒)</param>
        /// <param name="callback">执行完成后的回调</param>
        /// <returns>迭代器</returns>
        private IEnumerator StartExecute(float milliseconds, Action callback = null)
        {
            foreach (var cmd in m_baseCommands)
            {
                cmd.Do();
                yield return new WaitForSeconds(milliseconds / 1000f);
            }
            callback?.Invoke();
        }

        /// <summary>
        /// 执行命令
        /// </summary>
        /// <param name="milliseconds">执行间隔时间(毫秒)</param>
        /// <param name="callback">执行完成后的回调</param>
        public void Execute(float milliseconds, Action callback = null)
        {
            m_executeCoroutine = StartCoroutine(StartExecute(milliseconds, callback));
        }

        /// <summary>
        /// 执行命令
        /// </summary>
        /// <param name="milliseconds">执行间隔时间(毫秒)</param>
        /// <param name="beforeCall">开始执行前的回调</param>
        /// <param name="callback">执行完成后的回调</param>
        public void Execute(float milliseconds, Action beforeCall, Action callback = null)
        {
            beforeCall?.Invoke();
            Execute(milliseconds, callback);
        }

        /// <summary>
        /// 执行命令(无间隔时间)
        /// </summary>
        public void Execute()
        {
            Execute(0);
        }

        /// <summary>
        /// 停止执行
        /// </summary>
        public void Stop()
        {
            if(m_executeCoroutine != null)
                StopCoroutine(m_executeCoroutine);
            m_baseCommands.Clear();
        }

        /// <summary>
        /// 清空命令列表
        /// </summary>
        private void ClearCommands()
        {
            m_baseCommands.Clear();
        }
    }
}


