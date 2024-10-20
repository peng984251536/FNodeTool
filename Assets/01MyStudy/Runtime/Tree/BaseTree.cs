using System;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEngine.UIElements;
using UnityEditor;
#endif

namespace MyEditorView.Runtime
{
    public class BaseTree : ScriptableObject
    {
        public string treeName;

        public BaseNode rootNode;
        public BaseNode.State treeState;
        public List<BaseNode> Nodes = new List<BaseNode>(); //����������нڵ�

        bool running;
        public Action onUpdateEvent;
        public Action onStoppedEvent;

        //��ͣʱ��
        private float m_deleteTime = 0f;

        public virtual void ResetState()
        {
            running = false;
            treeState = BaseNode.State.Default;
            foreach (var baseNode in Nodes)
            {
                baseNode.Enable = true;
                baseNode.Init(this);
                baseNode.ResetState();
            }
            onUpdateEvent?.Invoke();
        }

        public virtual void OnStarted()
        {
            ResetState();
            treeState = BaseNode.State.Running;
            running = true;
        }

        public virtual BaseNode.State UpdateState()
        {
            if (running == false)
                OnStarted();
            if (treeState == BaseNode.State.Running)
            {
                if (m_deleteTime > 0)
                {
                    m_deleteTime -= Time.deltaTime;
                }
                else
                {
                    treeState = rootNode.UpdateState();
                    onUpdateEvent?.Invoke();
                }
            }

            // if (treeState == BaseNode.State.Success || treeState == BaseNode.State.Failure)
            //     OnStopped();
            return treeState;
        }

        public virtual void OnStopped()
        {
            running = false;
            onStoppedEvent?.Invoke();
            onStoppedEvent = null;
        }


        public void AddDeleteTime(float time)
        {
            m_deleteTime = time;
        }
        
        #region ��ȡ����

        public void InitTree(BaseNode rootNode,List<BaseNode> nodeDatas)
        {
            this.rootNode = rootNode;
            this.Nodes = nodeDatas;
        }
        
        private DialogueContainer m_currentContainer;
        
        // /// <summary>
        // /// �� view ���ر���Ľڵ�
        // /// </summary>
        // private BaseNode CreateNodes(NodeData nodeData)
        // {
        //     return null;
        // }
        
        #endregion
    }
}