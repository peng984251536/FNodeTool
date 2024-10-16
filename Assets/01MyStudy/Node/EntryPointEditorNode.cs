using System;
using UnityEditor.Experimental.GraphView;

namespace MyEditorView
{
    public class EntryPointEditorNode:EditorNodeBase
    {
        // private EntryPointNode()
        // {
        //     
        // }
        
        public EntryPointEditorNode(DialogueView graphView, string _guid=null): base(graphView,_guid)
        {
            if(_guid==null)
                _guid = Guid.NewGuid().ToString();
            this.GUID = _guid;
            var generatePort = EditorNodeBase.GeneratePort(this, Direction.Output, "Next",Port.Capacity.Multi);
            //generatePort.portName = "Next";
            //node.outputContainer.Add(generatePort);
            //var generatePort2 = GeneratePort(node, Direction.Input, "Input");
            //generatePort.portName = "Next";
            //node.outputContainer.Add(generatePort);

            
            
            this.RefreshExpandedState();
            this.RefreshPorts();
            this.SetPosition(new UnityEngine.Rect(100, 200, 100, 150));
            
            graphView.AddElement(this);
        }
        
    }
}