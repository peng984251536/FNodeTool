using UnityEngine;

namespace MyEditorView.Runtime
{

    [NodeName("DebugLogNode")]
    [NodePath("Base/DebugLogNode")]
    public class DebugLogNode : ActionNode
    {
        [HideInInspector][SerializeField]
        public string stringValue = string.Empty;
        
        [Header("≤‚ ‘")]
        public string stringValue2 = string.Empty;
        
        [HideInInspector][SerializeField]
        private string stringValue3 = string.Empty;

        protected override void DoAction()
        {
            Debug.Log($"DebugLogNode:{stringValue}");
        }

    }
}
