using System.Collections.Generic;

namespace MyEditorView.Runtime
{
    public abstract class ActionNode: BaseNode, IActionNode
    {
        protected override void OnReset()
        {
        }

        protected override void OnStart()
        {
            GetValue();
            DoAction();
        }

        protected override void OnStop()
        {
        }
        
        protected override State OnUpdate()
        {
           return State.Success;
        }

        public sealed override List<BaseNode> GetChildren()
        { 
            return new List<BaseNode>(); 
        }
        protected sealed override void GetValue()
        { 
            base.GetValue(); 
        }
        
        protected abstract void DoAction();
        public sealed override void AddChild(BaseNode baseNode) { }
        public sealed override void RemoveChild(BaseNode baseNode) { }
    }
}