using System;
using System.Collections.Generic;
using System.Linq;
using MyEditorView.Runtime;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;

namespace MyEditorView
{
    public class GraphSaveUtility
    {
        private DialogueContainer m_currentContainer;

        private DialogueView m_targetGraphView;

        //DialogueView 内部的连线
        private List<Edge> Edges => m_targetGraphView.edges.ToList();

        //DialogueView 内部的节点
        private List<EditorNodeBase> Nodes => m_targetGraphView.nodes.ToList().Cast<EditorNodeBase>().ToList();

        public static GraphSaveUtility GetInstance(DialogueView targetGraphView)
        {
            return new GraphSaveUtility()
            {
                m_targetGraphView = targetGraphView
            };
        }

        #region 保存数据

        public void SaveGraph(string fileName)
        {
            if (!Edges.Any()) return;

            var dialogueContainer = ScriptableObject.CreateInstance<DialogueContainer>();
            
            //保存 节点的连线信息
            var connectedPorts = Edges.Where((edge => true)).ToArray(); //拿到连线信息
            for (int i = 0; i < connectedPorts.Length; i++)
            {
                Edge curEdge = connectedPorts[i];
                var outputNode = curEdge.output.node as EditorNodeBase;
                var inputNode = curEdge.input.node as EditorNodeBase;

                //接口
                var outputCount = outputNode.outputContainer.FindPort(curEdge.output);
                var intputCount = inputNode.inputContainer.FindPort(curEdge.input);
                
                //保存连线数据
                dialogueContainer.NodeLinks.Add(new NodeLinkData()
                {
                    //OutPortName = curEdge.output.portName,
                    //InPortName = curEdge.input.portName,
                    
                    OutputNodeGuid = outputNode.GUID,
                    OutputNodeIndex = outputCount,
                    InputNodeGuid = inputNode.GUID,
                    InputNodeIndex = intputCount,
                });
            }

            //保存 节点数据
            foreach (var dialogueNode in Nodes)
            {
                string[] _OutPortName = new string[dialogueNode.outputContainer.childCount];
                for (int i = 0; i < _OutPortName.Length; i++)
                {
                    _OutPortName[i] = dialogueNode.outputContainer[i].name;
                }
                string[] _InPortName = new string[dialogueNode.inputContainer.childCount];
                for (int i = 0; i < _InPortName.Length; i++)
                {
                    _InPortName[i] = dialogueNode.inputContainer[i].name;
                }
                
                dialogueContainer.DialogueNodeDatas.Add(new DialogueNodeData()
                {
                    Guid = dialogueNode.GUID,
                    DialogueText = dialogueNode.DialogueText,
                    Position = dialogueNode.GetPosition().position,
                    
                    NodeType = dialogueNode.GetType().ToString(),
                    OutPortName = _OutPortName,
                    InPortName = _InPortName,
                    
                });
            }

            if (!AssetDatabase.IsValidFolder("Assets/Resources"))
                AssetDatabase.CreateFolder("Assets", "Resources");

            AssetDatabase.CreateAsset(dialogueContainer, $"Assets/Resources/{fileName}.asset");
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
        }
        /// <summary>
        /// 是否成功保存
        /// </summary>
        /// <param name="container"></param>
        /// <returns></returns>
        private bool SaveNodes(DialogueContainer container)
        {
            return false;
        }

        /// <summary>
        /// 保存熟悉
        /// </summary>
        private void SaveExposedProperties(DialogueContainer container)
        {
            
        }
        
        #endregion

        #region 读取数据
        public void LoadGraph(string fileName)
        {
            m_currentContainer = Resources.Load<DialogueContainer>(fileName);
            if (m_currentContainer == null)
            {
                EditorUtility.DisplayDialog("File Not Found", "找不到目标文件。", "Ok");
                return;
            }

            ClearGraph();
            CreateNodes();
            ConnectNodes();
        }
        
        /// <summary>
        /// 为节点进行连接
        /// </summary>
        private void ConnectNodes()
        {
            for (int i = 0; i < Nodes.Count; i++)
            {
                //找到 NodeLink-输出节点
                var connections = m_currentContainer.NodeLinks.Where
                (nodeLinkData => nodeLinkData.OutputNodeGuid == Nodes[i].GUID).ToList();
                
                for (int j = 0; j < connections.Count; j++)
                {
                    //NodeLink的输入节点
                    var targetNodeGuid = connections[j].InputNodeGuid;
                    //找到某个节点
                    var targetNode = Nodes.First(node => node.GUID == targetNodeGuid);
                    int intputIndex = connections[j].InputNodeIndex;
                    int outputIndex = connections[j].OutputNodeIndex;
                    LinkNodes(Nodes[i].outputContainer[outputIndex].Q<Port>(), (Port)targetNode.inputContainer[intputIndex]);
                    
                }
            }
        }
        
        /// <summary>
        /// 对两节点进行连线
        /// </summary>
        /// <param name="output"></param>
        /// <param name="input"></param>
        private void LinkNodes(Port output, Port input)
        {
            var tempEdge = new Edge
            {
                output = output,
                input = input
            };
            tempEdge?.input.Connect(tempEdge);
            tempEdge?.output.Connect(tempEdge);
            
            m_targetGraphView.Add(tempEdge);
        }
        
        /// <summary>
        /// 在 view 加载保存的节点
        /// </summary>
        private void CreateNodes()
        {
            //m_targetGraphView.GenerateEntryPointNode();
            foreach (var nodeData in m_currentContainer.DialogueNodeDatas)
            {
                //创建一个节点
                EditorNodeBase editorNodeBase;
                
                if (nodeData.InPortName.Length == 0||nodeData.NodeType.Contains(nameof(EntryPointEditorNode)))
                {
                    editorNodeBase = m_targetGraphView.GenerateEntryPointNode(nodeData.Guid);
                    editorNodeBase.SetPosition(new Rect(
                        nodeData.Position,
                        m_targetGraphView.DefaultNodeSize
                    ));
                    editorNodeBase.EntryPoint = true;
                }
                else//(nodeData.NodeType.Contains(nameof(DialogueNode)))
                {
                    editorNodeBase = m_targetGraphView.CreateNodeBase(nodeData.DialogueText, nodeData.Guid);
                    //nodeBase.outputContainer[0].name = 
                    editorNodeBase.SetPosition(new Rect(
                        nodeData.Position,
                        m_targetGraphView.DefaultNodeSize
                    ));
                    var button = new Button(() =>
                    {
                        editorNodeBase.AddChoicePort(editorNodeBase);
                    });
                    button.text = "New Port";
                    editorNodeBase.titleContainer.Add(button);
                    
                    for (int i = 0; i < nodeData.InPortName.Length; i++)
                    {
                        //string name = m_currentContainer.NodeLinks.FindPortName(nodeData.Guid,Direction.Input,i);
                        EditorNodeBase.GeneratePort(editorNodeBase, Direction.Input, nodeData.InPortName[i]);
                    }
                    for (int i = 0; i < nodeData.OutPortName.Length; i++)
                    {
                        //string name = m_currentContainer.NodeLinks.FindPortName(nodeData.Guid,Direction.Output,i);
                        editorNodeBase.AddChoicePort(editorNodeBase,nodeData.OutPortName[i]);
                    }
                }
                
                m_targetGraphView.AddElement(editorNodeBase);

                // //初始化节点的输出接口
                // var nodePorts = m_currentContainer.NodeLinks.Where(data => (nodeData.Guid == data.BaseNodeGuid))
                //     .ToList();
                // nodePorts.ForEach((data => m_targetGraphView.AddChoicePort(tempNode, data.PortName)));
            }
        }

        #region 创建对应的节点类型
        private EntryPointEditorNode CreateEntryNode(string dialogueName,string guid,Vector3 position)
        {
            EntryPointEditorNode editorNodeBase;
            editorNodeBase = m_targetGraphView.GenerateEntryPointNode(guid);
            editorNodeBase.SetPosition(new Rect(
                position,
                m_targetGraphView.DefaultNodeSize
            ));
            editorNodeBase.EntryPoint = true;
            return editorNodeBase;
        }
        

        #endregion
        
        /// <summary>
        /// 清楚当前的所有节点
        /// </summary>
        private void ClearGraph()
        {
            //Nodes.Find((node =>node.EntryPoint)).GUID = m_currentContainer.NodeLinks[0].BaseNodeGuid;


            // foreach (var node in Nodes)
            // {
            //     //if(node.EntryPoint) return;
            //     Func<Edge, bool> getEdgeFunc = edge => edge.input.node == node;
            //     Action<Edge> removeEdgeAction = edge =>
            //     {
            //         m_targetGraphView.RemoveElement(edge);
            //     };
            //     
            //     //移除连线
            //     Edges.Where(getEdgeFunc).ToList().ForEach(removeEdgeAction);
            // }
            foreach (var node in Nodes)
            {
                //if(node.EntryPoint) return;
                Func<Edge, bool> getEdgeFunc = edge => edge.input.node == node;
                Action<Edge> removeEdgeAction = edge => { m_targetGraphView.RemoveElement(edge); };

                //移除连线
                Edges.Where(getEdgeFunc).ToList().ForEach(removeEdgeAction);
            }

            Action<EditorNodeBase> removeNodeAction = Node => { m_targetGraphView.RemoveElement(Node); };
            Nodes.ForEach(removeNodeAction);
        }
        #endregion
    }
}