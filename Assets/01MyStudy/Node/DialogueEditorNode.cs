using System;
using System.Linq;
using UnityEditor.Experimental.GraphView;
using UnityEngine.UIElements;

namespace MyEditorView
{
    /// <summary>
    /// 编辑器节点
    /// </summary>
    public class DialogueEditorNode:EditorNodeBase
    {
        // private DialogueNode()
        // {
        //     
        // }
        
        public DialogueEditorNode(DialogueView graphView, string _guid=null): base(graphView,_guid)
        {
            if(_guid==null)
                _guid = Guid.NewGuid().ToString();
            this.GUID = _guid;
            m_graphView = graphView;
            //输入输出
            var inputPort1 = EditorNodeBase.GeneratePort(this, Direction.Input, "Input");
            var inputPort2 = EditorNodeBase.GeneratePort(this, Direction.Input, "Input2");
            //var inputPort3 = GeneratePort(dialogueNode, Direction.Output, "Onput", Port.Capacity.Multi);
            AddChoicePort(this);

            //节点的添加按钮事件
            var button = new Button(() =>
            {
                AddChoicePort(this);
            });
            button.text = "New Port";
            this.titleContainer.Add(button);

            
            
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