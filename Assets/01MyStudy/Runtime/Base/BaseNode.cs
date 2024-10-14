﻿using System;
using System.Collections.Generic;
using System.Reflection;
using _02TreeBehaviour;
using Unity.VisualScripting;
using UnityEngine;

namespace MyEditorView.Runtime
{
    [System.Serializable]
    public class BaseNodeLinkData
    {
        public BaseNode sourceNode;
        public string outputValueName;
        public string inputValueName;

        public BaseNodeLinkData(BaseNode sourceNode, string outputValueName, string inputValueName)
        {
            this.sourceNode = sourceNode;
            this.outputValueName = outputValueName;
            this.inputValueName = inputValueName;
        }
    }
    public abstract class BaseNode
    {
        public enum State
        {
            Default = 0,
            Success = 1,
            Failure = 2,
            Running = 3,
        }

        #region 属性

        private string m_uid = string.Empty;
        public string uid => m_uid;
        
        protected BaseTree m_Owner = null;
        public BaseTree Owner
        {
            get => m_Owner;
            set => m_Owner = value;
        }

        protected State m_nodeState;
        public State CurNodeState => m_nodeState;

        private bool m_started = false;
        public bool Started => m_started;
        
        private bool enable;
        public bool Enable { get => enable; set => enable = value; }

        #endregion

        /// <summary>
        /// 重置状态
        /// </summary>
        public void ResetState()
        {
            m_started = false;
            m_nodeState = State.Default;
            OnReset();
        }
        
        /// <summary>
        /// 更新状态
        /// </summary>
        /// <returns></returns>
        public virtual State UpdateState()
        {
            try
            {
                if (!m_started)
                {
                    OnStart();
                    m_started = true;
                }

                m_nodeState = OnUpdate();

                if (m_nodeState == State.Failure || m_nodeState == State.Success)
                {
                    OnStop();
                    m_started = false;
                }
            }
            catch (Exception ex)
            {
                m_nodeState = State.Failure;
                //Debug.LogError($"{m_started.name}:{m_started.treeName}");
                Debug.LogException(ex);
            }
            return m_nodeState;
        }
        
        
        
        
        
        
        
        
        
        protected virtual void GetValue()
        {
            OnGetValue();
        }
        
        
        
        protected abstract void OnReset();
        protected abstract void OnStart();
        protected abstract void OnStop();
        protected abstract State OnUpdate();
        protected virtual void OnGetValue() { }
        public abstract void AddChild(BaseNode baseNode);
        public abstract void RemoveChild(BaseNode baseNode);
        public abstract List<BaseNode> GetChildren();
        
        
    }
}