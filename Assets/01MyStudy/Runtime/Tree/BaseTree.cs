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
        public Dictionary<string,BaseNode> nodes = new Dictionary<string,BaseNode>(); //����������нڵ�

        bool running;
        public Action onUpdateEvent;
        public Action onStoppedEvent;

        //��ͣʱ��
        private float m_deleteTime = 0f;

        public virtual void ResetState()
        {
            running = false;
            treeState = BaseNode.State.Default;
            foreach (var baseNode in nodes.Values)
            {
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

            if (treeState == BaseNode.State.Success || treeState == BaseNode.State.Failure)
                OnStopped();
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
        
        private DialogueContainer m_currentContainer;
        
        public void LoadGraph(string fileName)
        {
            m_currentContainer = Resources.Load<DialogueContainer>(fileName);
            if (m_currentContainer == null)
            {
                EditorUtility.DisplayDialog("File Not Found", "�Ҳ���Ŀ���ļ���", "Ok");
                return;
            }
            
            CreateNodes();
        }
        
        /// <summary>
        /// �� view ���ر���Ľڵ�
        /// </summary>
        private void CreateNodes()
        {
            if(m_currentContainer==null)
                return;
            
            //m_targetGraphView.GenerateEntryPointNode();
            foreach (var nodeData in m_currentContainer.DialogueNodeDatas)
            {
                //����һ���ڵ�
                EditorNodeBase editorNodeBase;
                
                if (nodeData.InPortName.Length == 0||nodeData.NodeType.Contains(nameof(EntryPointEditorNode)))
                {
                    //nodes.Add(nodeBase.GUID,nodeData);
                }
                else//(nodeData.NodeType.Contains(nameof(DialogueNode)))
                {
                    // editorNodeBase = m_targetGraphView.CreateNodeBase(nodeData.DialogueText, nodeData.Guid);
                    // //nodeBase.outputContainer[0].name = 
                    // editorNodeBase.SetPosition(new Rect(
                    //     nodeData.Position,
                    //     m_targetGraphView.DefaultNodeSize
                    // ));
                    // var button = new Button(() =>
                    // {
                    //     editorNodeBase.AddChoicePort(editorNodeBase);
                    // });
                    // button.text = "New Port";
                    // editorNodeBase.titleContainer.Add(button);
                    
                    for (int i = 0; i < nodeData.InPortName.Length; i++)
                    {
                        //string name = m_currentContainer.NodeLinks.FindPortName(nodeData.Guid,Direction.Input,i);
                        //EditorNodeBase.GeneratePort(editorNodeBase, Direction.Input, nodeData.InPortName[i]);
                    }
                    for (int i = 0; i < nodeData.OutPortName.Length; i++)
                    {
                        //string name = m_currentContainer.NodeLinks.FindPortName(nodeData.Guid,Direction.Output,i);
                        //editorNodeBase.AddChoicePort(editorNodeBase,nodeData.OutPortName[i]);
                    }
                }
                
                //m_targetGraphView.AddElement(editorNodeBase);

                // //��ʼ���ڵ������ӿ�
                // var nodePorts = m_currentContainer.NodeLinks.Where(data => (nodeData.Guid == data.BaseNodeGuid))
                //     .ToList();
                // nodePorts.ForEach((data => m_targetGraphView.AddChoicePort(tempNode, data.PortName)));
            }
        }
        
        /// <summary>
        /// �� view ���ر���Ľڵ�
        /// </summary>
        private BaseNode CreateNodes(DialogueNodeData nodeData)
        {
            //Type type = Converter<nodeData.NodeType>
            
            //BaseNode baseNode = nodeData.NodeType.g
            return null;
        }
        
        #endregion
    }
}