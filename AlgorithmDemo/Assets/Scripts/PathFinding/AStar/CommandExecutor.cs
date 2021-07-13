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
        private List<BaseCommand<T>> m_baseCommands = new List<BaseCommand<T>>();

        private Coroutine m_executeCoroutine;

        public void AddCommand(BaseCommand<T> cmd)
        {
            m_baseCommands.Add(cmd);
        }

        public void RemoveCommand(BaseCommand<T> cmd)
        {
            m_baseCommands.Remove(cmd);
        }

        private IEnumerator StartExecute(float milliseconds, Action callback = null)
        {
            foreach (var cmd in m_baseCommands)
            {
                cmd.Do();
                yield return new WaitForSeconds(milliseconds / 1000f);
            }
            callback?.Invoke();
        }

        public void Execute(float milliseconds, Action callback = null)
        {
            m_executeCoroutine = StartCoroutine(StartExecute(milliseconds, callback));
        }

        public void Execute(float milliseconds, Action beforeCall, Action callback = null)
        {
            beforeCall?.Invoke();
            Execute(milliseconds, callback);
        }

        public void Execute()
        {
            Execute(0);
        }

        public void Stop()
        {
            if(m_executeCoroutine != null)
                StopCoroutine(m_executeCoroutine);
            m_baseCommands.Clear();
        }

        private void ClearCommands()
        {
            m_baseCommands.Clear();
        }
    }
}


