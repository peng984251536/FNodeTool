using UnityEngine;

namespace MyEditorView.Runtime
{

    public class DebugLogNode : ActionNode
    {
        public string stringValue;

        protected override void DoAction()
        {
            Debug.Log(stringValue);
        }
    }
}
