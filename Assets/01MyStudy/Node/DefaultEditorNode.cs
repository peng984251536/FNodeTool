using System;
using System.Linq;
using MyEditorView.Runtime;
using UnityEditor.Experimental.GraphView;
using UnityEngine.UIElements;

namespace MyEditorView
{
    /// <summary>
    /// 编辑器节点
    /// </summary>
    public class DefaultEditorNode:EditorNodeBase
    {
        public BaseNode BaseNode = null;
        
        // private DialogueNode()
        // {
        //     
        // }
        
        public DefaultEditorNode(GraphView graphView,BaseNode baseNode,string _guid=null): base(graphView,_guid)
        {
            if(_guid==null)
                _guid = Guid.NewGuid().ToString();
            this.GUID = _guid;
            this.BaseNode = baseNode;
            m_graphView = graphView;
            //输入输出
            var inputPort1 = EditorNodeBase.GeneratePort(this, Direction.Input, string.Empty);
            ActionNode actionNode = baseNode as ActionNode;
            if (actionNode == null)
            {
                var outputPort1 = EditorNodeBase.GeneratePort(this, Direction.Output, string.Empty,Port.Capacity.Multi);
            }
            
            
            
            this.RefreshExpandedState();
            this.RefreshPorts();
            this.SetPosition(new UnityEngine.Rect(UnityEngine.Vector2.zero, DefaultNodeSize));

            m_graphView.AddElement(this);
            
            this.RefreshExpandedState();
            this.RefreshPorts();
            this.SetPosition(new UnityEngine.Rect(100, 200, 100, 150));
            m_graphView.AddElement(this);
        }
        
    }
}