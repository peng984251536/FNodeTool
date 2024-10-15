using UnityEngine;

namespace MyEditorView.Runtime
{

    [NodeName("DebugLogNode")]
    [NodePath("Base/DebugLogNode")]
    public class DebugLogNode : ActionNode
    {
        public string stringValue;

        protected override void DoAction()
        {
            Debug.Log(stringValue);
        }

        public DebugLogNode(BaseTree baseTree) : base(baseTree)
        {
        }
    }
}
