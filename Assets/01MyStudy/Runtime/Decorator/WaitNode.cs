using UnityEngine;

namespace MyEditorView.Runtime
{
    
    [NodeName("WaitNode")]
    [NodePath("Base/WaitNode")]
    public class WaitNode : DecoratorNode
    {
        public float duration = 1;

        float startTime = 2f;

        protected override void DoAction()
        {
            Owner.AddDeleteTime(startTime);
        }
        protected override State OnUpdate()
        {
            if (Time.time - startTime > duration)
            {
                return child.Enable ? child.UpdateState() : State.Success;
            }
            else
                return State.Running;
        }
        
    }
}