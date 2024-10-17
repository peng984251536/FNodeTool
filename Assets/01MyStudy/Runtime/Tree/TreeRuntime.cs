using System;
using System.Collections.Generic;
using UnityEngine;

namespace MyEditorView.Runtime
{
    public class TreeRuntime:MonoBehaviour
    {
        [SerializeField]
        private DialogueContainer m_currentContainer;
        [SerializeField]
        public float DeleteTime = 0.5f;

        private BaseTree m_baseTree;
        private BaseNode m_rootNode;
        private List<BaseNode> allBaseNodes = new List<BaseNode>();
        
        private void Awake()
        {
            ReStartTree();
        }

        private float m_curDeleteTime = 0f;
        private void Update()
        {
            m_curDeleteTime += Time.deltaTime;
            if (m_curDeleteTime > DeleteTime)
            {
                m_curDeleteTime -= DeleteTime;
                LogicUpdate();
            }
        }

        [ContextMenu("ReStartTree")]
        public void ReStartTree()
        {
            if (m_currentContainer == null)
                return;
            if(m_baseTree==null)
                m_baseTree = m_currentContainer.CreateBaseTree();
            m_baseTree.OnStarted();
        }
        
        private void LogicUpdate()
        {
            //Debug.Log($"LogicUpdate");
            m_baseTree.UpdateState();
        }
    }
}