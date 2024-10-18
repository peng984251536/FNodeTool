using System;
using System.Collections.Generic;
using System.Linq;
using MyEditorView.Runtime;
using UnityEditor.Experimental.GraphView;
using UnityEngine.UIElements;

namespace MyEditorView
{
    public enum State
    {
        NoRun = 0,
        Running = 1,
        End = 2,
    }
    
    public class EditorNodeBase : Node
    {
        private UnityEngine.Vector2 m_defaultNodeSize = new UnityEngine.Vector2(100, 200);

        public UnityEngine.Vector2 DefaultNodeSize
        {
            get { return m_defaultNodeSize; }
        }

        protected GraphView m_graphView;
        public string GUID;
        public string DialogueText;
        public bool EntryPoint;
        public List<EditorNodeBase> childNodes = new List<EditorNodeBase>();
        public BaseNode BaseNode;

        public EditorNodeBase(GraphView graphView, string GUID)
        {
            m_graphView = graphView;
            this.GUID = (string.IsNullOrEmpty(GUID) || GUID == "") ? Guid.NewGuid().ToString() : GUID;
        }

        /// <summary>
        /// 给节点添加输入、输出接口
        /// </summary>
        /// <param name="editorNode">节点信息</param>
        /// <param name="portDirection">添加的接口类型</param>
        /// <param name="capacity"></param>
        /// <returns></returns>
        public static Port GeneratePort(EditorNodeBase editorNode, Direction portDirection, string name,
            Port.Capacity capacity = Port.Capacity.Single)
        {
            var generatePort = editorNode.InstantiatePort(Orientation.Horizontal, portDirection, capacity, typeof(float));
            generatePort.portName = name;
            if (portDirection == Direction.Input)
                editorNode.inputContainer.Add(generatePort);
            else if (portDirection == Direction.Output)
                editorNode.outputContainer.Add(generatePort);
            return generatePort;
        }

        /// <summary>
        /// 创建节点输出接口
        /// </summary>
        /// <param name="editorNodeBase"></param>
        public void AddChoicePort(EditorNodeBase editorNodeBase, string overridePortName = "")
        {
            var outputPortCount = editorNodeBase.outputContainer.Query("connector").ToList().Count;
            //输出接口
            var generatedPort = EditorNodeBase.GeneratePort(editorNodeBase, Direction.Output, $"Choice {outputPortCount + 1}",
                Port.Capacity.Multi);
            generatedPort.portName = string.IsNullOrEmpty(overridePortName)
                ? $"Choice {outputPortCount + 1}"
                : overridePortName;
            generatedPort.OnStartEdgeDragging();

            //移除
            // var oldLabel = generatedPort.contentContainer.Q<Label>("type");
            // generatedPort.contentContainer.Remove(oldLabel);
            //
            // var textField = new TextField
            // {
            //     name = string.Empty,
            //     value = generatedPort.portName
            // };
            // textField.RegisterValueChangedCallback(evt => generatedPort.portName = evt.newValue);
            // generatedPort.contentContainer.Add(new Label("  "));
            // generatedPort.contentContainer.Add(textField);

            //删除按钮
            var deleteButton = new Button((() => { RemovePort(editorNodeBase, generatedPort); }))
            {
                text = "X"
            };
            generatedPort.contentContainer.Add(deleteButton);

            editorNodeBase.RefreshPorts();
            editorNodeBase.RefreshExpandedState();
        }

        /// <summary>
        /// 移除节点
        /// </summary>
        /// <param name="editorNodeBase"></param>
        /// <param name="generatedPort"></param>
        public void RemovePort(EditorNodeBase editorNodeBase, Port generatedPort)
        {
            //找出对应的节点
            var targetEdge = m_graphView.edges.ToList().Where(edge =>
            (
                edge.output.portName == generatedPort.portName && edge.output.node == generatedPort.node
            )).ToList();

            //移除与改节点相关的连线
            if (targetEdge.Any())
            {
                var edge = targetEdge.First();
                edge.input.Disconnect(edge);
                m_graphView.RemoveElement(targetEdge.First());
            }

            editorNodeBase.outputContainer.Remove(generatedPort);
            editorNodeBase.RefreshPorts();
            editorNodeBase.RefreshExpandedState();
        }


        public virtual State OnUpdate()
        {
            return childNodes[0].OnUpdate();
        }
    }
}